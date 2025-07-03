using Application.Activities.Queries;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
           return await Mediator.Send(new GetActivityList.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivityDetail(string id)
        {
            var activity =  await Mediator.Send(new GetActivityDetails.Query(id));
            if (activity == null)
            {
                return NotFound();
            }
            return activity;
        }
    }
}
