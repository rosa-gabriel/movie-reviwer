using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListFavoritesAtPage
    {
        public class Query : IRequest<MoviePageResponse>
        {
            public string UserId { get; set; }
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
                    IQueryable<FavoriteEntry> search = _context.FavoriteEntries.Include(fe => fe.Film).Where(fe => fe.Fan.Id == request.UserId);
                    List<Movie> response = await search.OrderByDescending(fe => fe.FavoriteDate).Take(25).Skip((request.Page - 1) * 25).Select(fe => fe.Film).ToListAsync();
                    return new MoviePageResponse
                    {
                        movies = response,
                        count = (int)Math.Ceiling(((double)search.Count()) / 25),
                    };
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
        }
    }
}