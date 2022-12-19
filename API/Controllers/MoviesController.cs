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

        //Gets the a list of Movies at the given page.
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
        //Gets all the TagNames that the movie with the current id doesn't have.
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
        //Gets the full info in form of a MovieResponse of the movie with the given id.
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieResponse>> GetMovie(Guid id)
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
        //Adds the given movie to the database.
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
        //Changes info from a given movie in the database.
        [Authorize]
        [HttpPut("update")]
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
        //Deletes a movie from the database with the given id.
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
        //Return a list of Movies with containing the given filter in the name at the given page.
        [HttpGet("search/{filter}/{page}")]
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
