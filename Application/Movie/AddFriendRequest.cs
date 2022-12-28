using Application.Core;
using Application.Interfaces;
using Domain;
using Domain.Views;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class AddFriendRequest
    {
        public class Query : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
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
                User sender = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == this._userAccessor.GetUsername());
                if (sender == null) return Result<Unit>.Failure("Invalid User!");
                User receiver = await this._context.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
                if (receiver == null) return Result<Unit>.Failure("Invalid friend user!");
                if (sender == receiver) return Result<Unit>.Failure("User cannot friend self!");

                if ((await this._context.Friends.Where(f => (f.Receiver == receiver && f.Sender == sender)).FirstOrDefaultAsync()) != null) return Result<Unit>.Success(Unit.Value);
                Friend hasRecived = await this._context.Friends.Where(f => (f.Receiver == sender && f.Sender == receiver)).FirstOrDefaultAsync();
                if (hasRecived != null)
                {
                    return await this._mediator.Send(new ConfirmFriendRequest.Query { FriendshipId = hasRecived.Id });
                }

                Friend newFriend = new Friend
                {
                    Id = new Guid(),
                    Sender = sender,
                    Receiver = receiver,
                    Accepted = false
                };

                this._context.Friends.Add(newFriend);
                bool success = await this._context.SaveChangesAsync() > 0;
                if (!success) return Result<Unit>.Failure("Failed to send friend request!");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
