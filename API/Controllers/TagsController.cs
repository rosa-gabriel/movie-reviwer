using Application;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagsController : ControllerInit
    {
        public TagsController(IMediator mediator) : base(mediator) { }

        //Lists all the tags that are registered in the database.
        [HttpGet]
        public async Task<ActionResult<List<TagResponse>>> GetTags()
        {
            try
            {
                return Ok(await this._mediator.Send(new ListTags.Query()));
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
                return Ok(await this._mediator.Send(new FindTag.Query { Id = id }));
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
                return Ok(await this._mediator.Send(new ListMoviesWithTagAtPage.Query { Id = id, Page = page }));
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
                await this._mediator.Send(new AddTag.Query { NewTag = newTag });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}