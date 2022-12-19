using Application;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeedController : ControllerInit
    {
        public SeedController(DataContext context, IMediator mediator) : base(context, mediator)
        {
        }

        [HttpPost("movies")]
        public async Task<ActionResult> PostMovie(List<MovieResponse> newMovies)
        {
            try
            {
                foreach (MovieResponse mr in newMovies)
                {
                    await this._mediator.Send(new AddMovie.Query { NewMovie = mr });
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