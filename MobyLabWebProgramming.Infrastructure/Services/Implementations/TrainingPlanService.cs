using System.Net;
using Ardalis.Specification;
using MobyLabWebProgramming.Core.Constants;
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

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class TrainingPlanService : ITrainingPlanService
{

    private readonly IRepository<WebAppDatabaseContext> _repository;
    private readonly ILoginService _loginService;
    private readonly IMailService _mailService;

    public TrainingPlanService(IRepository<WebAppDatabaseContext> repository, ILoginService loginService, IMailService mailService)
    {
        _repository = repository;
        _loginService = loginService;
        _mailService = mailService;
    }

    public async Task<ServiceResponse<TrainingPlanDTO>> GetTrainingPlan(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new TrainingPlanProjectionSpec(id), cancellationToken);

        return result != null ?
            ServiceResponse<TrainingPlanDTO>.ForSuccess(result) :
            ServiceResponse<TrainingPlanDTO>.FromError(CommonErrors.UserNotFound);
    }

    public async Task<ServiceResponse<PagedResponse<TrainingPlanDTO>>> GetTrainingPlans(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new TrainingPlanProjectionSpec(pagination.Search), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse<PagedResponse<TrainingPlanDTO>>.ForSuccess(result);
    }

    public async Task<ServiceResponse<PagedResponse<TrainingPlanDTO>>> GetCurrentTrainerPlans(PaginationSearchQueryParams pagination, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new TrainingPlanProjectionSpec(pagination.Search, requestingUser.Id), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse<PagedResponse<TrainingPlanDTO>>.ForSuccess(result);
    }

    public async Task<ServiceResponse> AddTrainingPlan(TrainingPlanAddDTO plan, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && !(requestingUser.Role == UserRoleEnum.Admin || requestingUser.Role == UserRoleEnum.Trainer))
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only admin or trainers can add training plans!", ErrorCodes.CannotAdd));
        }

        await _repository.AddAsync(new TrainingPlan
        {
            Name = plan.Name,
            Description = plan.Description,
            DaysPerWeek = plan.DaysPerWeek,
            TrainerId = requestingUser.Id
        }, cancellationToken); // A new entity is created and persisted in the database.

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> SubscribeToPlan(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {

        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Client)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only clients can subscribe to training plans!", ErrorCodes.CannotUpdate));
        }
        

        var entity = await _repository.GetAsync(new UserSpec(requestingUser.Id), cancellationToken);

        if (entity != null) // Verify if the user is not found, you cannot update an non-existing entity.
        {
            entity.TrainingPlanId = id;

            await _repository.UpdateAsync(entity, cancellationToken); // Update the entity and persist the changes.
        }

        return ServiceResponse.ForSuccess();

    }

    public async Task<ServiceResponse> UnsubscribeFromPlan(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {

        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Client)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only clients can unsubscribe from a training plan!", ErrorCodes.CannotUpdate));
        }


        var entity = await _repository.GetAsync(new UserSpec(requestingUser.Id), cancellationToken);

        if (entity != null) // Verify if the user is not found, you cannot update an non-existing entity.
        {
            entity.TrainingPlanId = null;

            await _repository.UpdateAsync(entity, cancellationToken); // Update the entity and persist the changes.
        }

        return ServiceResponse.ForSuccess();

    }

    public async Task<ServiceResponse> Update(TrainingPlanUpdateDTO plan, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && !(requestingUser.Role == UserRoleEnum.Admin || requestingUser.Role == UserRoleEnum.Trainer))
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only admin or trainers can update training plans!", ErrorCodes.CannotUpdate));
        }


        var entity = await _repository.GetAsync(new TrainingPlanSpec(plan.Id), cancellationToken);

        if (entity.TrainerId != requestingUser.Id)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the owner can update this training plan!", ErrorCodes.CannotUpdate));
        }

        if (entity != null) // Verify if the user is not found, you cannot update an non-existing entity.
        {
            entity.Name = plan.Name ?? entity.Name;
            entity.Description = plan.Description ?? entity.Description;
            entity.DaysPerWeek = plan.DaysPerWeek ?? entity.DaysPerWeek;

            await _repository.UpdateAsync(entity, cancellationToken); // Update the entity and persist the changes.
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteTrainingPlan(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && !(requestingUser.Role == UserRoleEnum.Admin || requestingUser.Role == UserRoleEnum.Trainer))
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the trainer can delete a training plan!", ErrorCodes.CannotDelete));
        }

        var result = await _repository.GetAsync(new TrainingPlanProjectionSpec(id), cancellationToken);
        if (result == null) 
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Training plan doesn't exist!", ErrorCodes.EntityNotFound));
        }

        if (result.TrainerId != requestingUser.Id)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the owner can delete this training plan!", ErrorCodes.CannotDelete));
        }

        await _repository.DeleteAsync<TrainingPlan>(id, cancellationToken); // Delete the entity.

        return ServiceResponse.ForSuccess();
    }

}
