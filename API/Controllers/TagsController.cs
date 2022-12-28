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
        public async Task<ActionResult<List<TagResponse>>> GetTags()
        {
            Result<List<TagResponse>> result = await this._mediator.Send(new ListTags.Query());
            return this.ResultHandler(result);
        }
        //Gets one Tag with the given id.
        [HttpGet("{id}")]
        public async Task<ActionResult<TagName>> GetTag(Guid id)
        {
            Result<TagName> result = await this._mediator.Send(new FindTag.Query { Id = id });
            return this.ResultHandler(result);
        }
        //Gets the movies with the given id at the given page.
        [HttpGet("{id}/movies/{page}")]
        public async Task<ActionResult<MoviePageResponse>> GetMoviesFromTag(Guid id, int page)
        {
            Result<MoviePageResponse> result = await this._mediator.Send(new ListMoviesWithTagAtPage.Query { Id = id, Page = page });
            return this.ResultHandler(result);
        }
        //Adds a new Tag to the database with the TagName object given info.
        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<TagName>> PostTag(TagName newTag)
        {
            Result<TagName> result = await this._mediator.Send(new AddTag.Query { NewTag = newTag });
            return this.ResultHandler(result);
        }
    }
}