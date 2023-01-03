using Application.Core;
using Application.Validators;
using Domain;
using FluentValidation;
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

        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.MovieId).NotEmpty();
                RuleFor(x => x.DesiredBool).NotEmpty();
                RuleFor(x => x.user).SetValidator(new UserValidator());
            }
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

                Favorite favorite = await _context.Favorites.Where(fe => fe.User == request.user).Where(fe => fe.Movie.Id == request.MovieId).FirstOrDefaultAsync();

                if (!addFavorite)
                {
                    if (favorite == null) return Result<Unit>.Success(Unit.Value);

                    _context.Favorites.Remove(favorite);
                }
                else
                {
                    if (favorite != null) return Result<Unit>.Success(Unit.Value);

                    Favorite newFavorite = new Favorite
                    {
                        Id = new Guid(),
                        Movie = movie,
                        User = request.user,
                        FavoriteDate = DateTime.Now,
                    };

                    _context.Favorites.Add(newFavorite);
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
