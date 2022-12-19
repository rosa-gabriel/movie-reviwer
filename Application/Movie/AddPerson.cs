using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class AddPerson
    {
        public class Query : IRequest<Unit>
        {
            public Person NewPerson { get; set; }
        }

        public class Handler : IRequestHandler<Query, Unit>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Unit> Handle(Query request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrWhiteSpace(request.NewPerson.Name)) throw new Exception();
                if (string.IsNullOrWhiteSpace(request.NewPerson.ProfileImageUrl)) request.NewPerson.ProfileImageUrl = "";

                try
                {
                    _context.People.Add(request.NewPerson);
                    await _context.SaveChangesAsync();

                    return new Unit();
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
        }
    }
}
