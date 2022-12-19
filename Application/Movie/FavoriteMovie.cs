using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class FavoriteMovie
    {
        public class Query : IRequest<Unit>
        {
            public Guid MovieId { get; set; }
            public User user { get; set; }
            public bool DesiredBool { get; set; }
            public IMediator Mediator { get; set; }
        }

        public class Handler : IRequestHandler<Query, Unit>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Unit> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    Movie movie = await _context.Movies.FindAsync(request.MovieId);
                    if (movie == null) throw new Exception("Invalid movie Id");

                    bool addFavorite = request.DesiredBool && !(await request.Mediator.Send(new IsFavorite.Query { user = request.user, MovieId = request.MovieId }));

                    if (!addFavorite)
                    {
                        FavoriteEntry favoriteEntry = await _context.FavoriteEntries.Where(fe => fe.Fan == request.user).Where(fe => fe.Film.Id == request.MovieId).FirstOrDefaultAsync();

                        if (favoriteEntry == null) return new Unit();

                        _context.FavoriteEntries.Remove(favoriteEntry);
                    }
                    else
                    {
                        FavoriteEntry newFavoriteEntry = new FavoriteEntry
                        {
                            Id = new Guid(),
                            Film = movie,
                            Fan = request.user,
                            FavoriteDate = DateTime.Now,
                        };
                        _context.FavoriteEntries.Add(newFavoriteEntry);
                    }
                    await _context.SaveChangesAsync();
                    return new Unit();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
