using Application.Core;
using Application.Interfaces;
using Domain;
using Domain.Responses;
using Domain.Views;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListFavoritesAtPage
    {
        public class Query : IRequest<Result<MoviePageResponse>>
        {
            public string Username { get; set; }
            public int Page { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<MoviePageResponse>>
        {
            public readonly DataContext _context;
            public readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this._context = context;
                this._userAccessor = userAccessor;
            }

            public async Task<Result<MoviePageResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                User curentUser = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == this._userAccessor.GetUsername());

                if (request.Username != curentUser.UserName)
                {
                    User targetUser = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == request.Username);
                    if (targetUser == null) return Result<MoviePageResponse>.Failure("User doesn't exist!");

                    Friend friend = await this._context.Friends.Where(f => (f.Receiver == curentUser && f.Sender == targetUser) || (f.Receiver == targetUser && f.Sender == curentUser)).FirstOrDefaultAsync();
                    if (friend == null || !friend.Accepted) return Result<MoviePageResponse>.Failure("The users are not friends!");
                }

                IQueryable<FavoriteEntry> search = _context.FavoriteEntries.Include(fe => fe.Film).Where(fe => fe.Fan.UserName == request.Username);
                List<Movie> response = await search.OrderByDescending(fe => fe.FavoriteDate).Take(25).Skip((request.Page - 1) * 25).Select(fe => fe.Film).ToListAsync();
                MoviePageResponse moviePageResponse = new MoviePageResponse
                {
                    movies = response,
                    count = (int)Math.Ceiling(((double)search.Count()) / 25),
                };

                return Result<MoviePageResponse>.Success(moviePageResponse);
            }
        }
    }
}