using Microsoft.AspNetCore.Mvc;
using Domain;
using Persistence;
using Application;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("[Controller]")]

    public class MoviesController : ControllerInit
    {
        private readonly MovieLogic movieLogic;
        public MoviesController(DataContext context) : base(context)
        {
            movieLogic = new MovieLogic(_context);
        }

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

        [HttpGet]
        public async Task<ActionResult<List<Movie>>> GetMovies()
        {
            try
            {
                return Ok(await this.movieLogic.ListMovies());
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
                return Ok(await this.movieLogic.ListMoviesInfo(id));
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