using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application
{
    public class AddPerson
    {
        public class Query : IRequest<Result<Person>>
        {
            public Person NewPerson { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Person>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<Person>> Handle(Query request, CancellationToken cancellationToken)
            {
                request.NewPerson.Id = new Guid();

                _context.People.Add(request.NewPerson);
                bool success = await _context.SaveChangesAsync() > 0;

                if (!success) return Result<Person>.Failure("Failed to add person.");

                return Result<Person>.Success(request.NewPerson);
            }
        }
    }
}
