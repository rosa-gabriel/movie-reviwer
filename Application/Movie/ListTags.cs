using Application.Core;
using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListTags
    {
        public class Query : IRequest<Result<List<TagView>>> { }

        public class Handler : IRequestHandler<Query, Result<List<TagView>>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<List<TagView>>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<Tag> tags = await this._context.Tags.ToListAsync();

                List<TagView> response = new List<TagView>();

                foreach (Tag t in tags)
                {
                    TagView item = new TagView()
                    {
                        TagId = t.Id,
                        Name = t.Name,
                    };
                    item.Entries = t.Movies.Count();
                    response.Add(item);
                }
                return Result<List<TagView>>.Success(response);
            }
        }
    }
}