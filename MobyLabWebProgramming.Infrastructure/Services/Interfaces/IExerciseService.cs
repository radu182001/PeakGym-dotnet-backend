using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

public interface IExerciseService
{

    public Task<ServiceResponse<ExerciseDTO>> GetExercise(Guid id, CancellationToken cancellationToken = default);

    public Task<ServiceResponse<PagedResponse<ExerciseDTO>>> GetExercises(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);

    public Task<ServiceResponse<PagedResponse<ExerciseDTO>>> GetExercisesByTrainerPage(PaginationSearchQueryParams pagination, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);

    public Task<ServiceResponse<PagedResponse<ExerciseDTO>>> GetExercisesByTrainingPlanPage(PaginationSearchQueryParams pagination, Guid trainingPlanId, CancellationToken cancellationToken = default);

    public Task<ServiceResponse> Add(ExerciseAddDTO exercise, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);

    public Task<ServiceResponse> AddToTrainingPlan(ExerciseToTrainingPlanDTO data, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);

    //public Task<ServiceResponse> Update(TrainingPlanUpdateDTO plan, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);

    //public Task<ServiceResponse> Delete(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
}
