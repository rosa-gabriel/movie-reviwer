using Application.Core;
using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListMovieTags
    {
        public class Query : IRequest<Result<List<TagResponse>>>
        {
            public Movie movie { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<TagResponse>>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<List<TagResponse>>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<TagName> tags = await _context.TagEntries.Include(fe => fe.Tag).Where(fe => fe.Film == request.movie).Select(fe => fe.Tag).ToListAsync();
                List<TagResponse> tagResponses = new List<TagResponse>();
                foreach (TagName t in tags)
                {
                    var count = (await _context.TagEntries.Where(te => te.Tag == t).ToListAsync()).Count();
                    tagResponses.Add(new TagResponse()
                    {
                        TagId = t.Id,
                        Name = t.Name,
                        Entries = count
                    });
                }

                return Result<List<TagResponse>>.Success(tagResponses);
            }
        }
    }
}
