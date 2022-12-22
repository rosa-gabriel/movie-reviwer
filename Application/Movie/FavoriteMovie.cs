using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class FavoriteMovie
    {
        public class Query : IRequest<Result<Unit>>
        {
            public Guid MovieId { get; set; }
            public User user { get; set; }
            public bool DesiredBool { get; set; }
            public IMediator Mediator { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Unit>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<Unit>> Handle(Query request, CancellationToken cancellationToken)
            {
                Movie movie = await _context.Movies.FindAsync(request.MovieId);
                if (movie == null) return null;

                bool addFavorite = request.DesiredBool;

                FavoriteEntry favoriteEntry = await _context.FavoriteEntries.Where(fe => fe.Fan == request.user).Where(fe => fe.Film.Id == request.MovieId).FirstOrDefaultAsync();

                if (!addFavorite)
                {
                    if (favoriteEntry == null) return Result<Unit>.Success(Unit.Value);

                    _context.FavoriteEntries.Remove(favoriteEntry);
                }
                else
                {
                    if (favoriteEntry != null) return Result<Unit>.Success(Unit.Value);

                    FavoriteEntry newFavoriteEntry = new FavoriteEntry
                    {
                        Id = new Guid(),
                        Film = movie,
                        Fan = request.user,
                        FavoriteDate = DateTime.Now,
                    };

                    _context.FavoriteEntries.Add(newFavoriteEntry);
                }

                bool success = await _context.SaveChangesAsync() > 0;
                if (!success)
                {
                    return Result<Unit>.Failure("");
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
