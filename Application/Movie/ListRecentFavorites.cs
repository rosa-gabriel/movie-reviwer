using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListRecentFavorites
    {
        public class Query : IRequest<List<Movie>>
        {
            public User user { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<Movie>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<List<Movie>> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    List<Movie> movies = await _context.FavoriteEntries.Include(fe => fe.Film).Where(fe => fe.Fan == request.user).OrderByDescending(fe => fe.FavoriteDate).Select(fe => fe.Film).Take(5).ToListAsync();
                    return movies;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}