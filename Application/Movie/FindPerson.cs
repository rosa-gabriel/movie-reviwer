using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class FindPerson
    {
        public class Query : IRequest<Person>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Person>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Person> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    Person person = await this._context.People.Where(c => c.Id == request.Id).FirstOrDefaultAsync();
                    if (person == null) throw new Exception("Id not found");
                    return person;
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
        }
    }
}