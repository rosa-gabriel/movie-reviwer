using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListMoviesAtPage
    {
        public class Query : IRequest<MoviePageResponse>
        {
            public int Page { get; set; }
        }

        public class Handler : IRequestHandler<Query, MoviePageResponse>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<MoviePageResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    List<Movie> movies = await this._context.Movies.OrderByDescending(m => m.ReleaseDate).Skip((request.Page - 1) * 25).Take(25).ToListAsync();
                    if (request.Page > 1 && movies.Count == 0)
                    {
                        throw new Exception("Invalid Page");
                    }
                    return new MoviePageResponse
                    {
                        movies = movies,
                        count = (int)Math.Ceiling(((double)this._context.Movies.Count()) / 25),
                    };
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
    }
}