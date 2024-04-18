using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

public interface ITrainingPlanService
{

    public Task<ServiceResponse<TrainingPlanDTO>> GetTrainingPlan(Guid id, CancellationToken cancellationToken = default);

    public Task<ServiceResponse<PagedResponse<TrainingPlanDTO>>> GetTrainingPlans(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);

    public Task<ServiceResponse> AddTrainingPlan(TrainingPlanAddDTO plan, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);


    public Task<ServiceResponse> SubscribeToPlan(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);

    public Task<ServiceResponse> UnsubscribeFromPlan(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);

    public Task<ServiceResponse> Update(TrainingPlanUpdateDTO plan, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);


    public Task<ServiceResponse<PagedResponse<TrainingPlanDTO>>> GetCurrentTrainerPlans(PaginationSearchQueryParams pagination, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);

    public Task<ServiceResponse> DeleteTrainingPlan(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
}
