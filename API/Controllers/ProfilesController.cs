using Application.Activities.Queries;
using Application.Profiles.Commands;
using Application.Profiles.DTOs;
using Application.Profiles.Queries;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProfilesController : BaseApiController
    {
        [HttpPost("add-image")]
        public async Task<ActionResult<Image>> AddImage([FromForm] IFormFile file)
        {
            return HandleResult(await Mediator.Send(new AddImage.Command { File = file }));
        }

        [HttpGet("{UserId}/images")]
        public async Task<ActionResult<List<Image>>> GetProfileImages(string userId)
        {
            return HandleResult(await Mediator.Send(new GetProfileImages.Query { UserId = userId }));
        }

        [HttpDelete("{imageId}/images")]
        public async Task<ActionResult> DeleteImage(string imageId)
        {
            return HandleResult(await Mediator.Send(new DeleteImage.Command { ImageId = imageId }));
        }

        [HttpPut("{imageId}/setMain")]
        public async Task<ActionResult> SetMainImage(string imageId)
        {
            return HandleResult(await Mediator.Send(new SetMainImage.Command { ImageId = imageId }));
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserProfile>> GetUserProfile(string userId)
        {
            return HandleResult(await Mediator.Send(new GetUserProfile.Query { UserId = userId }));
        }

        [HttpPut]
        public async Task<ActionResult> EditProfile(EditProfile.Command command)
        {
            return HandleResult(await Mediator.Send(new EditProfile.Command(command.DisplayName, command.Bio)));
        }

        [HttpPost("{userId}/follow")]
        public async Task<ActionResult> FollowToggle(string userId)
        {
            return HandleResult(await Mediator.Send(new FollowToggle.Command(userId)));
        }

        [HttpGet("{userId}/follow-list")]
        public async Task<ActionResult<List<UserProfile>>> GetFollowings(string userId, string predicate = "followers")
        {
            return HandleResult(await Mediator.Send
                (new GetFollowings.Query(userId, predicate)));
        }
    }
}