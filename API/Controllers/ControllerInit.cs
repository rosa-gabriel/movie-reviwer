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
    }
}