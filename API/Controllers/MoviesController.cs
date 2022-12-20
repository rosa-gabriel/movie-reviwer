using Microsoft.AspNetCore.Mvc;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Domain.Responses;
using MediatR;
using Application;

namespace API.Controllers
{
    [ApiController]
    [Route("[Controller]")]

    public class MoviesController : ControllerInit
    {
        public MoviesController(IMediator mediator) : base(mediator) { }

        //Gets the a list of Movies at the given page.
        [HttpGet("page/{page}")]
        public async Task<ActionResult<MoviePageResponse>> GetMovies(int page)
        {
            try
            {
                return await this._mediator.Send(new ListMoviesAtPage.Query { Page = page });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        //Gets the full info in form of a MovieResponse of the movie with the given id.
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieResponse>> GetMovie(Guid id)
        {
            try
            {
                bool isLogged = User.Identity.IsAuthenticated;
                return Ok(await this._mediator.Send(new FindMovieInfo.Query { Id = id, Mediator = this._mediator }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        //Adds the given movie to the database.
        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult> PostMovie(MovieResponse newMovie)
        {
            try
            {
                await this._mediator.Send(new AddMovie.Query { NewMovie = newMovie });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        //Changes info from a given movie in the database.
        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult> PutMovie(MovieResponse newMovie)
        {
            try
            {
                await this._mediator.Send(new UpdateMovie.Query { NewMovie = newMovie });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        //Deletes a movie from the database with the given id.
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMovie(Guid id)
        {
            try
            {
                await this._mediator.Send(new RemoveMovie.Query { Id = id });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        //Return a list of Movies with containing the given filter in the name at the given page.
        [HttpGet("search/{filter}/{page}")]
        public async Task<ActionResult<MoviePageResponse>> GetMovieSearch(string filter, int page)
        {
            try
            {
                return Ok(await this._mediator.Send(new ListMoviesSearchAtPage.Query { Filter = filter, Page = page }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
