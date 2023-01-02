using Application.Core;
using Application.Interfaces;
using Domain;
using Domain.Views;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ConfirmFriendRequest
    {
        public class Query : IRequest<Result<Unit>>
        {
            public Guid FriendshipId { get; set; }
        }
        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.FriendshipId).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, Result<Unit>>
        {
            public readonly DataContext _context;
            public readonly IUserAccessor _userAccessor;
            public readonly IMediator _mediator;
            public Handler(DataContext context, IUserAccessor userAccessor, IMediator mediator)
            {
                this._context = context;
                this._userAccessor = userAccessor;
                this._mediator = mediator;
            }

            public async Task<Result<Unit>> Handle(Query request, CancellationToken cancellationToken)
            {
                User user = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == this._userAccessor.GetUsername());

                Friend friendship = await this._context.Friends.FirstOrDefaultAsync(f => f.Id == request.FriendshipId);
                if (friendship == null) return Result<Unit>.Failure("Users don't have a friend request!");
                if (friendship.Receiver != user) return Result<Unit>.Unauthorize();
                if (friendship.Accepted) return Result<Unit>.Success(Unit.Value);

                friendship.Accepted = true;

                bool success = await this._context.SaveChangesAsync() > 0;
                if (!success) return Result<Unit>.Failure("Failed to confirm friend request!");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
