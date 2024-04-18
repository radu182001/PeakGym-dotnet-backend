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

public class ExerciseService : IExerciseService
{

    private readonly IRepository<WebAppDatabaseContext> _repository;

    public ExerciseService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<ExerciseDTO>> GetExercise(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new ExerciseProjectionSpec(id), cancellationToken);

        return result != null ?
            ServiceResponse<ExerciseDTO>.ForSuccess(result) :
            ServiceResponse<ExerciseDTO>.FromError(CommonErrors.UserNotFound);
    }

    public async Task<ServiceResponse<PagedResponse<ExerciseDTO>>> GetExercises(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new ExerciseProjectionSpec(pagination.Search), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse<PagedResponse<ExerciseDTO>>.ForSuccess(result);
    }

    public async Task<ServiceResponse<PagedResponse<ExerciseDTO>>> GetExercisesByTrainerPage(PaginationSearchQueryParams pagination, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new ExerciseProjectionSpec(pagination.Search, requestingUser.Id, true), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse<PagedResponse<ExerciseDTO>>.ForSuccess(result);
    }

    public async Task<ServiceResponse<PagedResponse<ExerciseDTO>>> GetExercisesByTrainingPlanPage(PaginationSearchQueryParams pagination, Guid trainingPlanId, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new ExerciseProjectionSpec(pagination.Search, trainingPlanId, false), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse<PagedResponse<ExerciseDTO>>.ForSuccess(result);
    }

    public async Task<ServiceResponse> Add(ExerciseAddDTO exercise, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && !(requestingUser.Role == UserRoleEnum.Admin || requestingUser.Role == UserRoleEnum.Trainer))
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only admin or trainers can create exercises!", ErrorCodes.CannotAdd));
        }

        await _repository.AddAsync(new Exercise
        {
            Name = exercise.Name,
            MuscleGroup = exercise.MuscleGroup,
            SetsNo = exercise.SetsNo,
            TrainerId = requestingUser.Id
        }, cancellationToken); // A new entity is created and persisted in the database.

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> AddToTrainingPlan(ExerciseToTrainingPlanDTO data, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && !(requestingUser.Role == UserRoleEnum.Admin || requestingUser.Role == UserRoleEnum.Trainer))
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only admin or trainers can add exercises to training plans!", ErrorCodes.CannotAdd));
        }

        var entity = await _repository.GetAsync(new ExerciseSpec(data.ExerciseId), cancellationToken);

        if (entity.TrainerId != requestingUser.Id)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the owner can add exercises to this training plan!", ErrorCodes.CannotUpdate));
        }

        await _repository.AddAsync(new TrainingPlanExercise
        {
            TrainingPlanId = data.TrainingPlanId,
            ExerciseId = data.ExerciseId,
            Weekday = data.Weekday
        }, cancellationToken); // A new entity is created and persisted in the database.

        return ServiceResponse.ForSuccess();
    }

}
