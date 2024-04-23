using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Extensions;
using MobyLabWebProgramming.Infrastructure.Services.Implementations;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProgressLogController : AuthorizedController
{
    private readonly IProgressLogService _progressLogService;

    public ProgressLogController(IUserService userService, IProgressLogService progressLogService) : base(userService) // Also, you may pass constructor parameters to a base class constructor and call as specific constructor from the base class.
    {
        _progressLogService = progressLogService;
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<ProgressLogDTO>>> GetById([FromRoute] Guid id)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _progressLogService.GetProgressLog(id)) :
            this.ErrorMessageResult<ProgressLogDTO>(currentUser.Error);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<ProgressLogDTO>>>> GetPage([FromQuery] PaginationQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _progressLogService.GetProgressLogs(pagination, 0, currentUser.Result)) :
            this.ErrorMessageResult<PagedResponse<ProgressLogDTO>>(currentUser.Error);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<ProgressLogDTO>>>> GetPageThisWeek([FromQuery] PaginationQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _progressLogService.GetProgressLogs(pagination, 3, currentUser.Result)) :
            this.ErrorMessageResult<PagedResponse<ProgressLogDTO>>(currentUser.Error);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<ProgressLogDTO>>>> GetPageThisMonth([FromQuery] PaginationQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _progressLogService.GetProgressLogs(pagination, 2, currentUser.Result)) :
            this.ErrorMessageResult<PagedResponse<ProgressLogDTO>>(currentUser.Error);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<ProgressLogDTO>>>> GetPageThisYear([FromQuery] PaginationQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _progressLogService.GetProgressLogs(pagination, 1, currentUser.Result)) :
            this.ErrorMessageResult<PagedResponse<ProgressLogDTO>>(currentUser.Error);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] ProgressLogAddDTO log)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _progressLogService.Add(log, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] ProgressLogUpdateDTO log) // The FromBody attribute indicates that the parameter is deserialized from the JSON body.
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _progressLogService.Update(log, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id) // The FromRoute attribute will bind the id from the route to this parameter.
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _progressLogService.Delete(id, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

}
