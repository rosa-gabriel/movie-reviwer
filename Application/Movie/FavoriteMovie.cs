using Application.Core;
using Application.Interfaces;
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
            public bool DesiredBool { get; set; }
        }

        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.MovieId).NotEmpty();
                RuleFor(x => x.DesiredBool).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, Result<Unit>>
        {
            public readonly DataContext _context;
            public readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this._context = context;
                this._userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Query request, CancellationToken cancellationToken)
            {
                User user = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == this._userAccessor.GetUsername());
                if (user == null) return Result<Unit>.Unauthorize();

                Movie movie = await _context.Movies.FindAsync(request.MovieId);
                if (movie == null) return null;

                bool addFavorite = request.DesiredBool;

                Favorite favorite = await _context.Favorites.Where(fe => fe.User == user).Where(fe => fe.Movie.Id == request.MovieId).FirstOrDefaultAsync();

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
                        User = user,
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
