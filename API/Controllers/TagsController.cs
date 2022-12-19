using System.Security.Claims;
using API.DTOs;
using API.Extensions;
using Domain;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagsController : ControllerInit
    {
        private readonly TokenService _tokenService;
        public TagsController(DataContext context) : base(context) { }

        //Lists all the tags that are registered in the database.
        [HttpGet]
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
        //Gets one Tag with the given id.
        [HttpGet("{id}")]
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
        //Gets the movies with the given id at the given page.
        [HttpGet("{id}/movies/{page}")]
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
        //Adds a new Tag to the database with the TagName object given info.
        [Authorize]
        [HttpPost("create")]
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
    }
}