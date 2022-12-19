using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class IsFavorite
    {
        public class Query : IRequest<bool>
        {
            public Guid MovieId { get; set; }
            public User user { get; set; }
        }

        public class Handler : IRequestHandler<Query, bool>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    FavoriteEntry favoriteEntry = await _context.FavoriteEntries.FirstOrDefaultAsync(fe => fe.Film.Id == request.MovieId && fe.Fan == request.user);
                    return favoriteEntry != null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
