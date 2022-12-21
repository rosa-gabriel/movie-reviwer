using Application.Core;
using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListMoviesSearchAtPage
    {
        public class Query : IRequest<Result<MoviePageResponse>>
        {
            public string Filter { get; set; }
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
                IQueryable<Movie> search = _context.Movies.Where(m => m.Name.ToLower().Contains(request.Filter.ToLower()));
                List<Movie> response = await search.OrderByDescending(ce => ce.ReleaseDate).Take(25).Skip((request.Page - 1) * 25).ToListAsync();
                return Result<MoviePageResponse>.Success(new MoviePageResponse
                {
                    movies = response,
                    count = (int)Math.Ceiling(((double)search.Count()) / 25),
                });
            }
        }
    }
}