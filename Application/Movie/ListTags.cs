using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListTags
    {
        public class Query : IRequest<List<TagResponse>> { }

        public class Handler : IRequestHandler<Query, List<TagResponse>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<List<TagResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    List<TagName> tags = await this._context.TagNames.ToListAsync();
                    List<TagResponse> response = new List<TagResponse>();
                    foreach (TagName t in tags)
                    {
                        TagResponse item = new TagResponse()
                        {
                            TagId = t.Id,
                            Name = t.Name,
                        };
                        List<TagEntry> entries = await this._context.TagEntries.ToListAsync();
                        item.Entries = entries.Count();
                        response.Add(item);
                    }
                    return response;
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
        }
    }
}