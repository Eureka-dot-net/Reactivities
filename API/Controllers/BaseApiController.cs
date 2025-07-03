using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : Controller
    {
        private IMediator? _mediator;

        protected IMediator Mediator => 
            _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>()
            ?? throw new InvalidOperationException("The mediator service is unavailable");
    }
}
