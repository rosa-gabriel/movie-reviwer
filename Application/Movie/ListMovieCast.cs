using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListMovieCast
    {
        public class Query : IRequest<Result<List<CastResponse>>>
        {
            public Movie movie { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<CastResponse>>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<List<CastResponse>>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<CastEntry> cast = await _context.CastEntries.Include(ce => ce.Person).Where(ce => ce.Film == request.movie).ToListAsync();
                List<CastResponse> castResponses = new List<CastResponse>();
                foreach (CastEntry c in cast)
                {
                    castResponses.Add(new CastResponse()
                    {
                        PersonId = c.Person.Id,
                        Name = c.Person.Name,
                        Role = c.Role
                    });
                }
                return Result<List<CastResponse>>.Success(castResponses);
            }
        }
    }
}
