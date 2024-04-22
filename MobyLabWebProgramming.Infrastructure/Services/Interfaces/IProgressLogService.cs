using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

public interface IProgressLogService
{

    public Task<ServiceResponse<ProgressLogDTO>> GetProgressLog(Guid id, CancellationToken cancellationToken = default);

    public Task<ServiceResponse<PagedResponse<ProgressLogDTO>>> GetProgressLogs(PaginationQueryParams pagination, int flag, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Add(ProgressLogAddDTO log, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);

}
