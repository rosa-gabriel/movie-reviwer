using Application;
using Application.Core;
using Domain;
using Domain.Responses;
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
        public async Task<ActionResult<List<TagView>>> GetTags()
        {
            Result<List<TagView>> result = await this._mediator.Send(new ListTags.Query());
            return this.ResultHandler(result);
        }
        //Gets one Tag with the given id.
        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTag(Guid id)
        {
            Result<Tag> result = await this._mediator.Send(new FindTag.Query { Id = id });
            return this.ResultHandler(result);
        }
        //Gets the movies with the given id at the given page.
        [HttpGet("{id}/movies/{page}")]
        public async Task<ActionResult<MoviePageView>> GetMoviesFromTag(Guid id, int page)
        {
            Result<MoviePageView> result = await this._mediator.Send(new ListMoviesWithTagAtPage.Query { Id = id, Page = page });
            return this.ResultHandler(result);
        }
        //Adds a new Tag to the database with the Tag object given info.
        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<Tag>> PostTag(Tag newTag)
        {
            Result<Tag> result = await this._mediator.Send(new AddTag.Query { NewTag = newTag });
            return this.ResultHandler(result);
        }
    }
}