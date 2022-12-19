using System.Security.Claims;
using API.DTOs;
using API.Extensions;
using Domain;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CastController : ControllerInit
    {
        private readonly TokenService _tokenService;
        public CastController(DataContext context) : base(context) { }

        //Lists all the cast people that are registered in the database
        [HttpGet]
        public async Task<ActionResult<List<Person>>> GetCast()
        {
            try
            {
                return Ok(await this.movieLogic.ListCast());
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
                return Ok(await this.movieLogic.FindPerson(id));
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
                return Ok(await this.movieLogic.ListMoviesFromPersonAtPage(id, page));
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
                Person responsePerson = await movieLogic.AddPerson(newPerson);
                return Ok(responsePerson);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}