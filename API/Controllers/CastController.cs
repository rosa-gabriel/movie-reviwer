using Application;
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
            try
            {
                return Ok(await this._mediator.Send(new ListCast.Query()));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        //Gets one Person with the given id.
        [HttpGet("person/{id}")]
        public async Task<ActionResult<Person>> GetPerson(Guid id)
        {
            try
            {
                return Ok(await this._mediator.Send(new FindPerson.Query { Id = id }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        //Gets the movies that the person with the given id at the given page.
        [HttpGet("{id}/movies/{page}")]
        public async Task<ActionResult<MoviePageResponse>> GetMoviesFromPerson(Guid id, int page)
        {
            try
            {
                return Ok(await this._mediator.Send(new ListMoviesWithPersonAtPage.Query { Id = id, Page = page }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        //Creates a new Person with the person object given.
        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<Person>> PostPerson(Person newPerson)
        {
            try
            {
                await this._mediator.Send(new AddPerson.Query { NewPerson = newPerson });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}