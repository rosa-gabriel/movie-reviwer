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
                return Ok(await this.movieLogic.ListMovies());
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("tag/{id}")]
        public async Task<ActionResult<TagName>> GetTag(int id)
        {
            try
            {
                return Ok(await this.movieLogic.FindTag(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("person/{id}")]
        public async Task<ActionResult<Cast>> GetPerson(int id)
        {
            try
            {
                return Ok(await this.movieLogic.FindPerson(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("from/tag/{id}")]
        public async Task<ActionResult<TagName>> GetMoviesFromTag(int id)
        {
            try
            {
                return Ok(await this.movieLogic.ListMoviesFromTag(id));
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
                return Ok(await this.movieLogic.ListTags());
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
                await movieLogic.AddTag(newTag);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("person")]
        public async Task<ActionResult> PostPerson(Cast newPerson)
        {
            try
            {
                await movieLogic.AddPerson(newPerson);
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
                await this.movieLogic.AddMovie(newMovie);
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
                return Ok(await this.movieLogic.ListMoviesInfo(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("cast")]
        public async Task<ActionResult<List<Cast>>> GetCast()
        {
            try
            {
                return Ok(await this.movieLogic.ListCast());
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

    }
}