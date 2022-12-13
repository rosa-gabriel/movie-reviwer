using Microsoft.AspNetCore.Mvc;
using Domain;
using Persistence;
using Microsoft.AspNetCore.Authorization;
using Domain.Responses;
using Application;

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

        [HttpGet("{id}/missingTags")]
        public async Task<ActionResult<List<TagResponse>>> GetMissingTags(Guid id)
        {
            try
            {
                return Ok(await this.movieLogic.ListMissingTags(id));
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


        [HttpGet("/Tag/{id}/movies/{page}")]
        public async Task<ActionResult<Movie>> GetMoviesFromTag(Guid id, int page)
        {
            try
            {
                return Ok(await this.movieLogic.ListMoviesFromTagAtPage(id, page));
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

        [HttpGet("/Person/{id}/movies/{page}")]
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

        [Authorize]
        [HttpPut("/Update/movie")]
        public async Task<ActionResult> PutMovie(MovieResponse newMovie)
        {
            try
            {
                await this.movieLogic.UpdateMovie(newMovie);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMovie(Guid id)
        {
            try
            {
                await this.movieLogic.RemoveMovie(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("/Movies/search/{filter}/{page}")]
        public async Task<ActionResult<MoviePageResponse>> GetMovieSearch(string filter, int page)
        {
            try
            {
                return Ok(await this.movieLogic.ListMoviesSearchAtPage(filter, page));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
