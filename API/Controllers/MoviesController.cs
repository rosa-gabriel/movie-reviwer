using Microsoft.AspNetCore.Mvc;
using Domain;
using Persistence;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Application;

namespace API.Controllers
{
    [ApiController]
    [Route("[Controller]")]

    public class MoviesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly MovieLogic movieLogic;
        public MoviesController(DataContext context)
        {
            _context = context;
            movieLogic = new MovieLogic(_context);
        }

        [HttpGet]
        public async Task<ActionResult<List<Movie>>> GetMovies()
        {
            try
            {
                return Ok(await this.movieLogic.GetAllMovies());
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostMovie(Movie newMovie)
        {
            try
            {
                await this.movieLogic.PostMovie(newMovie);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieResponse>> getMovie(int id)
        {
            try
            {
                return Ok(await this.movieLogic.getAllMovieInfo(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

    }
}