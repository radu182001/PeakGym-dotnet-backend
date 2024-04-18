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
public class TrainingPlanController : AuthorizedController
{
    private readonly ITrainingPlanService _trainingPlanService;

    public TrainingPlanController(IUserService userService, ITrainingPlanService trainingPlanService) : base(userService) // Also, you may pass constructor parameters to a base class constructor and call as specific constructor from the base class.
    {
        _trainingPlanService = trainingPlanService;
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<TrainingPlanDTO>>> GetById([FromRoute] Guid id)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _trainingPlanService.GetTrainingPlan(id)) :
            this.ErrorMessageResult<TrainingPlanDTO>(currentUser.Error);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] TrainingPlanAddDTO trainingPlan)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _trainingPlanService.AddTrainingPlan(trainingPlan, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Subscribe([FromBody] TrainingPlanSubscribeDTO plan) // The FromBody attribute indicates that the parameter is deserialized from the JSON body.
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _trainingPlanService.SubscribeToPlan(plan.Id, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Unsubscribe([FromBody] TrainingPlanSubscribeDTO plan) // The FromBody attribute indicates that the parameter is deserialized from the JSON body.
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _trainingPlanService.UnsubscribeFromPlan(plan.Id, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] TrainingPlanUpdateDTO plan)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _trainingPlanService.Update(plan with
            {
                Name = !string.IsNullOrWhiteSpace(plan.Name) ? plan.Name : null,
                Description = !string.IsNullOrWhiteSpace(plan.Description) ? plan.Description : null,
                DaysPerWeek = plan.DaysPerWeek > 0 ? plan.DaysPerWeek : null
            }, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<TrainingPlanDTO>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _trainingPlanService.GetTrainingPlans(pagination)) :
            this.ErrorMessageResult<PagedResponse<TrainingPlanDTO>>(currentUser.Error);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<TrainingPlanDTO>>>> GetCurrentTrainerPlansPage([FromQuery] PaginationSearchQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _trainingPlanService.GetCurrentTrainerPlans(pagination, currentUser.Result)) :
            this.ErrorMessageResult<PagedResponse<TrainingPlanDTO>>(currentUser.Error);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _trainingPlanService.DeleteTrainingPlan(id, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

}
