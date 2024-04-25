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

public class ProgressLogService : IProgressLogService
{

    private readonly IRepository<WebAppDatabaseContext> _repository;

    public ProgressLogService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }


    public async Task<ServiceResponse<ProgressLogDTO>> GetProgressLog(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new ProgressLogProjectionSpec(id), cancellationToken);

        return result != null ?
            ServiceResponse<ProgressLogDTO>.ForSuccess(result) :
            ServiceResponse<ProgressLogDTO>.FromError(CommonErrors.ProgressLogNotFound);
    }

    public async Task<ServiceResponse<PagedResponse<ProgressLogDTO>>> GetProgressLogs(PaginationQueryParams pagination, int flag, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new ProgressLogProjectionSpec(requestingUser.Id, flag), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse<PagedResponse<ProgressLogDTO>>.ForSuccess(result);
    }

    public async Task<ServiceResponse> Add(ProgressLogAddDTO log, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Client)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only clients can add progress!", ErrorCodes.CannotAdd));
        }

        if (requestingUser.TrainingPlanId == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "You are not subscribed to any plan!", ErrorCodes.CannotAdd));
        }

        await _repository.AddAsync(new ProgressLog
        {
            ClientId = requestingUser.Id,
            TrainingPlanId = requestingUser.TrainingPlanId ?? Guid.Empty,
            Date = DateTime.Now,
            Weekday = log.Weekday
        }, cancellationToken); // A new entity is created and persisted in the database.

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> Update(ProgressLogUpdateDTO log, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && !(requestingUser.Role == UserRoleEnum.Admin || requestingUser.Role == UserRoleEnum.Client))
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only admin or clients can update exercises!", ErrorCodes.CannotUpdate));
        }


        var entity = await _repository.GetAsync(new ProgressLogSpec(log.Id), cancellationToken);

        if (entity.ClientId != requestingUser.Id)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the owner canupdate this log!", ErrorCodes.CannotUpdate));
        }

        if (entity != null) // Verify if the user is not found, you cannot update an non-existing entity.
        {
            entity.TrainingPlanId = log.TrainingPlanId ?? entity.TrainingPlanId;
            entity.Weekday = log.Weekday ?? entity.Weekday;

            await _repository.UpdateAsync(entity, cancellationToken); // Update the entity and persist the changes.
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> Delete(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && !(requestingUser.Role == UserRoleEnum.Admin || requestingUser.Role == UserRoleEnum.Client))
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the client can delete this log!", ErrorCodes.CannotDelete));
        }

        var result = await _repository.GetAsync(new ProgressLogProjectionSpec(id), cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Progress log doesn't exist!", ErrorCodes.EntityNotFound));
        }

        if (result.ClientId != requestingUser.Id)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the owner can delete this log!", ErrorCodes.CannotDelete));
        }

        await _repository.DeleteAsync<ProgressLog>(id, cancellationToken); // Delete the entity.

        return ServiceResponse.ForSuccess();
    }

}
