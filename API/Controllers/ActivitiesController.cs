using System;
using Application.Activities.Commands;
using Application.Activities.DTO;
using Application.Activities.Queries;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ActivitiesController : BaseApiController
{
    //[AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<ActivityDto>>> GetActivities()
    {
        return await Mediator.Send(new GetActivityList.Query());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ActivityDto>> GetActivityDetail(string id)
    {
        return HandleResult(await Mediator.Send(new GetActivityDetails.Query { Id = id }));

    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateActivity(CreateActivityDto activityDto)
    {
        return HandleResult(await Mediator.Send(new CreateActivity.Command { ActivityDto = activityDto }));
    }

    [HttpPut("{id}")]
    [Authorize(Policy= "IsActivityHost")]
    public async Task<ActionResult> EditActivity(string id, EditActivityDto activityDto)
    {
        activityDto.Id = id;
        return HandleResult(await Mediator.Send(new EditActivity.Command { ActivityDto = activityDto }));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "IsActivityHost")]
    public async Task<ActionResult> DeleteActivity(string id)
    {
        return HandleResult(await Mediator.Send(new DeleteActivity.Command { Id = id }));
    }

    [HttpPost("attend/{activityId}")]
    public async Task<ActionResult> UpdateAttendance(string activityId)
    {
        return HandleResult(await Mediator.Send(new UpdateAttendance.Command(activityId, true)));
    }

    [HttpDelete("attend/{activityId}")]
    public async Task<ActionResult> RemoveAttendance(string activityId)
    {
        return HandleResult(await Mediator.Send(new UpdateAttendance.Command(activityId, false)));
    }
}
