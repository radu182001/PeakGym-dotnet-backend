using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Core.Specifications;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using System.Net;
using System.Numerics;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class GoalService : IGoalService
{

    private readonly IRepository<WebAppDatabaseContext> _repository;

    public GoalService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<GoalDTO>> GetGoal(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new GoalProjectionSpec(id), cancellationToken);

        return result != null ?
            ServiceResponse<GoalDTO>.ForSuccess(result) :
            ServiceResponse<GoalDTO>.FromError(CommonErrors.GoalNotFound);
    }

    public async Task<ServiceResponse<PagedResponse<GoalDTO>>> GetGoals(PaginationQueryParams pagination, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new GoalProjectionSpec(requestingUser.Id, 0), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse<PagedResponse<GoalDTO>>.ForSuccess(result);
    }

    public async Task<ServiceResponse> Add(GoalAddDTO goal, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Client)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only clients can add goals!", ErrorCodes.CannotAdd));
        }

        await _repository.AddAsync(new Goal
        {
            ClientId = requestingUser.Id,
            Name = goal.Name,
            TargetValue = goal.TargetValue,
            CurrentValue = goal.CurrentValue,
        }, cancellationToken); // A new entity is created and persisted in the database.

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> Update(GoalUpdateDTO goal, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && !(requestingUser.Role == UserRoleEnum.Admin || requestingUser.Role == UserRoleEnum.Client))
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only admin or clients can update goals!", ErrorCodes.CannotUpdate));
        }


        var entity = await _repository.GetAsync(new GoalSpec(goal.Id), cancellationToken);

        if (entity.ClientId != requestingUser.Id)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the owner can update this goal!", ErrorCodes.CannotUpdate));
        }

        if (entity != null) // Verify if the user is not found, you cannot update an non-existing entity.
        {
            entity.Name = goal.Name ?? entity.Name;
            entity.TargetValue = goal.TargetValue ?? entity.TargetValue;
            entity.CurrentValue = goal.CurrentValue ?? entity.CurrentValue;

            await _repository.UpdateAsync(entity, cancellationToken); // Update the entity and persist the changes.
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> Delete(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && !(requestingUser.Role == UserRoleEnum.Admin || requestingUser.Role == UserRoleEnum.Client))
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the client can delete this goal!", ErrorCodes.CannotDelete));
        }

        var result = await _repository.GetAsync(new GoalProjectionSpec(id), cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Goal doesn't exist!", ErrorCodes.EntityNotFound));
        }

        if (result.ClientId != requestingUser.Id)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the owner can delete this goal!", ErrorCodes.CannotDelete));
        }

        await _repository.DeleteAsync<Goal>(id, cancellationToken); // Delete the entity.

        return ServiceResponse.ForSuccess();
    }

}
