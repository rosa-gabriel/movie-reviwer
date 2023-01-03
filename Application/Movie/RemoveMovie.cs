using Application.Core;
using Domain;
using Domain.Responses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class RemoveMovie
    {
        public class Query : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }
        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotEmpty();
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
                Movie movie = await this._context.Movies.Where(m => m.Id == request.Id).FirstOrDefaultAsync();
                if (movie == null) return null;

                IQueryable<CastRole> cast = this._context.CastRoles.Where(ce => ce.Movie == movie);
                IQueryable<Favorite> favorites = this._context.Favorites.Where(f => f.Movie == movie);
                IQueryable<Comment> comments = this._context.Comments.Where(c => c.Movie == movie);

                this._context.CastRoles.RemoveRange(cast);
                this._context.Favorites.RemoveRange(favorites);
                this._context.Comments.RemoveRange(comments);
                await _context.SaveChangesAsync();

                this._context.Remove(movie);
                bool success = await _context.SaveChangesAsync() > 0;

                if (!success) return Result<Unit>.Failure("Failed to delete the movie.");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
