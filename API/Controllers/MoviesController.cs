using Microsoft.AspNetCore.Mvc;
using Domain;
using Persistence;
using Application;
using Microsoft.AspNetCore.Authorization;
using Domain.Responses;

namespace API.Controllers
{
    [ApiController]
    [Route("[Controller]")]

    public class MoviesController : ControllerInit
    {
        public MoviesController(DataContext context) : base(context) { }
        //Tags Requests
        [HttpGet("/Tags")]
        public async Task<ActionResult<List<TagResponse>>> GetTags()
        {
            try
            {
                return Ok(await this.movieLogic.ListTags());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("/Tag/{id}")]
        public async Task<ActionResult<TagName>> GetTag(Guid id)
        {
            try
            {
                return Ok(await this.movieLogic.FindTag(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpPost("/Create/tag")]
        public async Task<ActionResult> PostTag(TagName newTag)
        {
            try
            {
                TagName responseTag = await movieLogic.AddTag(newTag);
                return Ok(responseTag);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("/Tag/{id}/movies")]
        public async Task<ActionResult<Movie>> GetMoviesFromTag(Guid id)
        {
            try
            {
                return Ok(await this.movieLogic.ListMoviesFromTag(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //Cast Requests

        [HttpGet("/Cast")]
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

        [HttpGet("/Person/{id}")]
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

        [HttpGet("/Person/{id}/movies")]
        public async Task<ActionResult<Movie>> GetMoviesFromPerson(Guid id)
        {
            try
            {
                return Ok(await this.movieLogic.ListMoviesFromPerson(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpPost("/Create/person")]
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

        //Movie Requests

        [HttpGet("{page}")]
        public async Task<ActionResult<MoviePageResponse>> GetMovies(int page)
        {
            try
            {
                return await this.movieLogic.ListMoviesAtPage(page);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("/Movie/{id}")]
        public async Task<ActionResult<MovieResponse>> getMovie(Guid id)
        {
            try
            {
                bool isLogged = User.Identity.IsAuthenticated;
                return Ok(await this.movieLogic.FindMoviesInfo(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpPost("/Create/movie")]
        public async Task<ActionResult> PostMovie(MovieResponse newMovie)
        {
            try
            {
                await this.movieLogic.AddMovie(newMovie);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}