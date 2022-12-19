using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class FindTag
    {
        public class Query : IRequest<TagName>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, TagName>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<TagName> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    TagName tag = await this._context.TagNames.Where(tn => tn.Id == request.Id).FirstAsync();
                    if (tag == null) throw new Exception("Id not found");
                    return tag;
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
        }
    }
}