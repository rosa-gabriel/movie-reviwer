using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListCast
    {
        public class Query : IRequest<List<Person>> { }

        public class Handler : IRequestHandler<Query, List<Person>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<List<Person>> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    List<Person> response = await this._context.People.ToListAsync();
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