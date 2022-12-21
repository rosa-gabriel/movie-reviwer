using Application.Core;
using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListMoviesWithPersonAtPage
    {
        public class Query : IRequest<Result<MoviePageResponse>>
        {
            public Guid Id { get; set; }
            public int Page { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<MoviePageResponse>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<MoviePageResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<Movie> response = await _context.CastEntries.Include(ce => ce.Film).Where(ce => ce.Person.Id == request.Id).Select(ce => ce.Film).OrderByDescending(ce => ce.ReleaseDate).Take(25).Skip((request.Page - 1) * 25).ToListAsync();
                return Result<MoviePageResponse>.Success(new MoviePageResponse
                {
                    movies = response,
                    count = (int)Math.Ceiling(((double)this._context.CastEntries.Where(ce => ce.Person.Id == request.Id).Count()) / 25),
                });
            }
        }
    }
}