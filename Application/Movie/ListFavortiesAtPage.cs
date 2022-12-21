using Application.Core;
using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListFavoritesAtPage
    {
        public class Query : IRequest<Result<MoviePageResponse>>
        {
            public string UserId { get; set; }
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
                IQueryable<FavoriteEntry> search = _context.FavoriteEntries.Include(fe => fe.Film).Where(fe => fe.Fan.Id == request.UserId);
                List<Movie> response = await search.OrderByDescending(fe => fe.FavoriteDate).Take(25).Skip((request.Page - 1) * 25).Select(fe => fe.Film).ToListAsync();
                MoviePageResponse moviePageResponse = new MoviePageResponse
                {
                    movies = response,
                    count = (int)Math.Ceiling(((double)search.Count()) / 25),
                };

                return Result<MoviePageResponse>.Success(moviePageResponse);
            }
        }
    }
}