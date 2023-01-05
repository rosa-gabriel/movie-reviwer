using Application.Core;
using Application.Interfaces;
using Domain;
using Domain.Views;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using FluentValidation;

namespace Application
{
    public class FindProfile
    {
        public class Query : IRequest<Result<ProfileView>>
        {
            public string Id { get; set; }
        }
        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
            }
        }

        public class Handler : IRequestHandler<Query, Result<ProfileView>>
        {
            public readonly UserManager<User> _userManager;
            public readonly DataContext _context;
            public readonly IMediator _mediator;
            public readonly IUserAccessor _userAccessor;
            public Handler(UserManager<User> userManager, DataContext context, IMediator mediator, IUserAccessor userAccessor)
            {
                this._userManager = userManager;
                this._context = context;
                this._mediator = mediator;
                this._userAccessor = userAccessor;
            }

            public async Task<Result<ProfileView>> Handle(Query request, CancellationToken cancellationToken)
            {
                User self = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());
                if (self == null) return Result<ProfileView>.Unauthorize();

                ProfileView profile;
                User user = await _userManager.FindByIdAsync(request.Id);
                if (user == null) return null;

                profile = user.ToProfileView();

                profile.RecentFavorites = (await this._mediator.Send(new ListRecentFavorites.Query { user = user })).Value;

                profile.IsLogedIn = user.Id.Equals(self.Id);
                Friend friend = _context.Friends.Where(f => (f.Receiver == self && f.Sender == user && f.Accepted) || (f.Receiver == user && f.Sender == self)).FirstOrDefault();

                if (friend == null)
                {
                    profile.IsLogedIn = false;
                    profile.HasRequested = false;
                }
                else
                {
                    profile.IsFriend = friend.Accepted;
                    profile.HasRequested = true;
                }

                return Result<ProfileView>.Success(profile);
            }
        }
    }
}
