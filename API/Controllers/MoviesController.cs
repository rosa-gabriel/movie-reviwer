using Microsoft.AspNetCore.Mvc;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Domain.Responses;
using MediatR;
using Application;
using Domain.Views;
using Application.Core;

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
            Result<MoviePageResponse> result = await this._mediator.Send(new ListMoviesAtPage.Query { Page = page });

            return this.ResultHandler<MoviePageResponse>(result);
        }
        //Gets the full info in form of a MovieResponse of the movie with the given id.
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieResponse>> GetMovie(Guid id)
        {
            bool isLogged = User.Identity.IsAuthenticated;
            Result<MovieResponse> result = await this._mediator.Send(new FindMovieInfo.Query { Id = id, Mediator = this._mediator });
            return this.ResultHandler(result);
        }

        [HttpGet("{id}/comments")]
        public async Task<ActionResult<List<CommentView>>> GetUserComments(Guid id)
        {
            Result<List<CommentView>> result = await this._mediator.Send(new ListMovieComments.Query { MovieId = id });
            return this.ResultHandler(result);
        }
        //Adds the given movie to the database.
        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult> PostMovie(MovieResponse newMovie)
        {
            Result<Unit> result = await this._mediator.Send(new AddMovie.Query { NewMovie = newMovie });
            return this.ResultHandler(result);
        }
        //Changes info from a given movie in the database.
        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult> PutMovie(MovieResponse newMovie)
        {
            Result<Unit> result = await this._mediator.Send(new UpdateMovie.Query { NewMovie = newMovie });
            return this.ResultHandler(result);
        }
        //Deletes a movie from the database with the given id.
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMovie(Guid id)
        {
            Result<Unit> result = await this._mediator.Send(new RemoveMovie.Query { Id = id });
            return this.ResultHandler(result);
        }
        //Return a list of Movies with containing the given filter in the name at the given page.
        [HttpGet("search/{filter}/{page}")]
        public async Task<ActionResult<MoviePageResponse>> GetMovieSearch(string filter, int page)
        {
            Result<MoviePageResponse> result = await this._mediator.Send(new ListMoviesSearchAtPage.Query { Filter = filter, Page = page });
            return this.ResultHandler(result);
        }

    }
}
