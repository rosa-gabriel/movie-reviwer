using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application
{
    public class AddPerson
    {
        public class Query : IRequest<Result<Unit>>
        {
            public Person NewPerson { get; set; }
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
                _context.People.Add(request.NewPerson);
                bool success = await _context.SaveChangesAsync() > 0;

                if (!success) return Result<Unit>.Failure("Failed to add person.");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
