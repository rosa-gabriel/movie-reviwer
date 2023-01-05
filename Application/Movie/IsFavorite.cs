using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class IsFavorite
    {
        public class Query : IRequest<Result<bool>>
        {
            public Guid MovieId { get; set; }
        }

        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.MovieId).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, Result<bool>>
        {
            public readonly DataContext _context;
            public readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this._context = context;
                this._userAccessor = userAccessor;
            }

            public async Task<Result<bool>> Handle(Query request, CancellationToken cancellationToken)
            {
                User user = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == this._userAccessor.GetUsername());
                if (user == null) return Result<bool>.Unauthorize();

                Favorite favorite = await _context.Favorites.FirstOrDefaultAsync(fe => fe.Movie.Id == request.MovieId && fe.User == user);
                return Result<bool>.Success(favorite != null);
            }
        }
    }
}
