using Application.Core;
using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListCast
    {
        public class Query : IRequest<Result<List<Person>>> { }

        public class Handler : IRequestHandler<Query, Result<List<Person>>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<List<Person>>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<Person> response = await this._context.People.ToListAsync();
                return Result<List<Person>>.Success(response);
            }
        }
    }
}