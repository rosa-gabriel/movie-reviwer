using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application
{
    public class AddTag
    {
        public class Query : IRequest<Result<TagName>>
        {
            public TagName NewTag { get; set; }
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
                request.NewTag.Id = new Guid();

                _context.TagNames.Add(request.NewTag);
                bool success = await _context.SaveChangesAsync() > 0;

                if (!success) return Result<TagName>.Failure("Failed to add tag.");

                return Result<TagName>.Success(request.NewTag);
            }
        }
    }
}
