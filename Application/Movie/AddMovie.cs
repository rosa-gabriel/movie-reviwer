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
    public class AddMovie
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

                Movie movie = request.NewMovie.ToMovie();
                _context.Movies.Add(movie);

                foreach (NewTag nt in request.NewMovie.tags)
                {
                    Tag currentTag = await this._context.Tags.FirstOrDefaultAsync(nt => nt.Id == nt.Id);
                    if (currentTag == null) return Result<Unit>.Failure("One of the given tags are not valid!");
                    movie.Tags.Add(currentTag);
                }


                foreach (NewCastView ncv in request.NewMovie.castMembers)
                {
                    CastRole newCastRole = new CastRole();
                    newCastRole.Person = await this._context.People.Where(p => p.Name == ncv.Name).FirstAsync();
                    newCastRole.Role = ncv.Role;
                    newCastRole.Movie = movie;
                    _context.CastRoles.Add(newCastRole);
                }

                bool success = await _context.SaveChangesAsync() > 0;

                if (!success) return Result<Unit>.Failure("Failed to add Movie.");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
