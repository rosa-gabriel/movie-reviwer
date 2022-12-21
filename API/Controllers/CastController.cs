using Application;
using Application.Core;
using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CastController : ControllerInit
    {
        public CastController(IMediator mediator) : base(mediator) { }

        //Lists all the cast people that are registered in the database
        [HttpGet]
        public async Task<ActionResult<List<Person>>> GetCast()
        {
            Result<List<Person>> result = await this._mediator.Send(new ListCast.Query());
            return this.ResultHandler(result);
        }
        //Gets one Person with the given id.
        [HttpGet("person/{id}")]
        public async Task<ActionResult<Person>> GetPerson(Guid id)
        {
            Result<Person> result = await this._mediator.Send(new FindPerson.Query { Id = id });
            return this.ResultHandler(result);
        }
        //Gets the movies that the person with the given id at the given page.
        [HttpGet("{id}/movies/{page}")]
        public async Task<ActionResult<MoviePageResponse>> GetMoviesFromPerson(Guid id, int page)
        {
            Result<MoviePageResponse> result = await this._mediator.Send(new ListMoviesWithPersonAtPage.Query { Id = id, Page = page });
            return this.ResultHandler(result);
        }
        //Creates a new Person with the person object given.
        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult> PostPerson(Person newPerson)
        {
            Result<Unit> result = await this._mediator.Send(new AddPerson.Query { NewPerson = newPerson });
            return this.ResultHandler(result);
        }
    }
}