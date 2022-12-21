using Application.Core;
using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class FindTag
    {
        public class Query : IRequest<Result<TagName>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<TagName>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<TagName>> Handle(Query request, CancellationToken cancellationToken)
            {
                TagName tag = await this._context.TagNames.Where(tn => tn.Id == request.Id).FirstAsync();
                if (tag == null) return null;

                return Result<TagName>.Success(tag);
            }
        }
    }
}