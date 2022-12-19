using Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace API.Controllers
{
    public class ControllerInit : ControllerBase
    {
        protected readonly DataContext _context;
        protected readonly IMediator _mediator;
        public ControllerInit(DataContext context, IMediator mediator)
        {
            this._mediator = mediator;
            _context = context;
        }
    }
}