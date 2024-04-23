using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

public interface IGoalService
{

    public Task<ServiceResponse<GoalDTO>> GetGoal(Guid id, CancellationToken cancellationToken = default);

    public Task<ServiceResponse<PagedResponse<GoalDTO>>> GetGoals(PaginationQueryParams pagination, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Add(GoalAddDTO goal, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);

    public Task<ServiceResponse> Update(GoalUpdateDTO goal, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);

    public Task<ServiceResponse> Delete(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);

}
