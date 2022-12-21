using Application.Core;
using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class FindPerson
    {
        public class Query : IRequest<Result<Person>>
        {
            public Guid Id { get; set; }
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
                Person person = await this._context.People.Where(c => c.Id == request.Id).FirstOrDefaultAsync();
                if (person == null) return null;

                return Result<Person>.Success(person);
            }
        }
    }
}