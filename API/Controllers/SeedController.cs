using Application;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeedController : ControllerInit
    {
        private readonly MovieLogic movieLogic;
        public SeedController(DataContext context) : base(context)
        {
            movieLogic = new MovieLogic(this._context);
        }

        [HttpPost("movies")]
        public async Task<ActionResult> PostMovie(List<MovieResponse> newMovies)
        {
            try
            {
                foreach (MovieResponse mr in newMovies)
                {
                    await this.movieLogic.AddMovie(mr);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}