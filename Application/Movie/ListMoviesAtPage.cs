using Application.Core;
using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListMoviesAtPage
    {
        public class Query : IRequest<Result<MoviePageResponse>>
        {
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
                List<Movie> movies = await this._context.Movies.OrderByDescending(m => m.ReleaseDate).Skip((request.Page - 1) * 25).Take(25).ToListAsync();
                if (request.Page > 1 && movies.Count == 0) Result<MoviePageResponse>.Failure("Invalid Page");

                return Result<MoviePageResponse>.Success(new MoviePageResponse
                {
                    movies = movies,
                    count = (int)Math.Ceiling(((double)this._context.Movies.Count()) / 25),
                }
                );
            }
        }
    }
}