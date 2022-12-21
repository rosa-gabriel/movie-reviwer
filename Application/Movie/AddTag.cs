using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application
{
    public class AddTag
    {
        public class Query : IRequest<Result<Unit>>
        {
            public TagName NewTag { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Unit>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<Unit>> Handle(Query request, CancellationToken cancellationToken)
            {
                _context.TagNames.Add(request.NewTag);
                bool success = await _context.SaveChangesAsync() > 0;

                if (!success) return Result<Unit>.Failure("Failed to add tag.");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
