using Application.Core;
using Application.Interfaces;
using Domain;
using Domain.Responses;
using Domain.Views;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListFavoritesAtPage
    {
        public class Query : IRequest<Result<MoviePageView>>
        {
            public string Username { get; set; }
            public int Page { get; set; }
        }
        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Page).NotEmpty().GreaterThanOrEqualTo(1);
                RuleFor(x => x.Username).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, Result<MoviePageView>>
        {
            public readonly DataContext _context;
            public readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this._context = context;
                this._userAccessor = userAccessor;
            }

            public async Task<Result<MoviePageView>> Handle(Query request, CancellationToken cancellationToken)
            {
                User curentUser = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == this._userAccessor.GetUsername());
                if (curentUser == null) return Result<MoviePageView>.Unauthorize();

                if (request.Username != curentUser.UserName)
                {
                    User targetUser = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == request.Username);
                    if (targetUser == null) return null;

                    Friend friend = await this._context.Friends.Where(f => (f.Receiver == curentUser && f.Sender == targetUser) || (f.Receiver == targetUser && f.Sender == curentUser)).FirstOrDefaultAsync();
                    if (friend == null || !friend.Accepted) return Result<MoviePageView>.Failure("The users are not friends!");
                }

                IQueryable<Favorite> search = _context.Favorites.Include(fe => fe.Movie).Where(fe => fe.User.UserName == request.Username);
                List<Movie> response = await search.OrderByDescending(fe => fe.FavoriteDate).Take(25).Skip((request.Page - 1) * 25).Select(fe => fe.Movie).ToListAsync();
                MoviePageView moviePageView = new MoviePageView(response, (int)Math.Ceiling(((double)search.Count()) / 25));

                return Result<MoviePageView>.Success(moviePageView);
            }
        }
    }
}