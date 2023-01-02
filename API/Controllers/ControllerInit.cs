using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ControllerInit : ControllerBase
    {
        protected readonly IMediator _mediator;
        public ControllerInit(IMediator mediator)
        {
            this._mediator = mediator;
        }

        protected ActionResult ResultHandler<T>(Result<T> result)
        {
            if (result == null) return NotFound();
            if (result.IsSuccess) return Ok(result.Value);
            if (result.Unauthorized) return Unauthorized();
            return BadRequest(result.Error);
        }
    }
}