using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

public interface ICommentService
{

    public Task<ServiceResponse<CommentDTO>> GetComment(Guid id, CancellationToken cancellationToken = default);

    public Task<ServiceResponse<PagedResponse<CommentDTO>>> GetCommentsForTrainingPlan(PaginationQueryParams pagination, Guid trainingPlanId, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Add(CommentAddDTO comment, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);

    public Task<ServiceResponse> Update(CommentUpdateDTO comment, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);

    public Task<ServiceResponse> Delete(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);

}
