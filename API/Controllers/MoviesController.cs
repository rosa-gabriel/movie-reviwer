using Microsoft.AspNetCore.Mvc;
using Domain;
using Persistence;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Application;
using Newtonsoft.Json.Linq;

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

        [HttpGet("tags")]
        public async Task<ActionResult<List<TagResponse>>> GetTags()
        {
            try
            {
                return Ok(await this.movieLogic.GetAllTags());
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("tags")]
        public async Task<ActionResult> PostTags(TagName newTag)
        {
            try
            {
                await movieLogic.PostTag(newTag);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostMovie(MovieResponse newMovie)
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