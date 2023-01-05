using Application.Core;
using Application.Interfaces;
using Application.Validators;
using Domain;
using Domain.Responses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class UpdateMovie
    {
        public class Query : IRequest<Result<Unit>>
        {
            public NewMovieView NewMovie { get; set; }
        }
        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
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
                if (!await this._userAccessor.CheckIfCurrentUserIsAdmin()) return Result<Unit>.Unauthorize();

                Movie movie = this._context.Movies.Include(m => m.Tags).Include(m => m.Cast).FirstOrDefault(m => m.Id == request.NewMovie.Id);

                if (movie == null) return null;

                movie.CoverUrl = request.NewMovie.CoverUrl;
                movie.Name = request.NewMovie.Name;
                movie.ReleaseDate = request.NewMovie.ReleaseDate;

                movie.Tags.Clear();

                foreach (NewTag nt in request.NewMovie.tags)
                {
                    Tag currentTag = await this._context.Tags.FirstOrDefaultAsync(t => t.Id == nt.Id);
                    if (currentTag == null) return Result<Unit>.Failure("One of the given tags are not valid!");
                    currentTag.Movies.Add(movie);
                    movie.Tags.Add(currentTag);
                }

                List<CastRole> oldCast = await this._context.CastRoles.Where(cr => cr.Movie.Id == movie.Id).ToListAsync();
                this._context.CastRoles.RemoveRange(oldCast);

                foreach (NewCastView ncv in request.NewMovie.castMembers)
                {
                    CastRole newCastRole = new CastRole();
                    Person person = await this._context.People.Where(p => p.Name == ncv.Name).FirstAsync();
                    if (person == null) return Result<Unit>.Failure("One of the given cast members are not valid!");
                    newCastRole.Person = person;
                    newCastRole.Role = ncv.Role;
                    newCastRole.Movie = movie;
                    _context.CastRoles.Add(newCastRole);
                }

                bool success = await this._context.SaveChangesAsync() > 0;

                if (!success) return Result<Unit>.Failure("Failed to update the movie.");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
