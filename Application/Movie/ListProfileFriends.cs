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
    public class ListProfileFriends
    {
        public class Query : IRequest<Result<FriendsListView>> { }

        public class Handler : IRequestHandler<Query, Result<FriendsListView>>
        {
            public readonly DataContext _context;
            public readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this._context = context;
                this._userAccessor = userAccessor;
            }

            public async Task<Result<FriendsListView>> Handle(Query request, CancellationToken cancellationToken)
            {
                User user = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());
                if (user == null) return Result<FriendsListView>.Failure("Invalid user!");


                List<Friend> allFriends = await this._context.Friends.Where(f => f.Sender == user).Include(f => f.Receiver).Include(f => f.Sender).ToListAsync();
                allFriends.AddRange(this._context.Friends.Where(f => f.Receiver == user).Include(f => f.Sender).Include(f => f.Receiver));

                FriendsListView flv = new FriendsListView();

                foreach (Friend f in allFriends)
                {
                    if (f.Accepted)
                    {
                        flv.Friends.Add(f.ToFriendView(user));
                    }
                    else
                    {
                        flv.FriendRequests.Add(f.ToFriendView(user));
                    }
                }


                return Result<FriendsListView>.Success(flv);
            }
        }
    }
}
