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
    public class RemoveFriend
    {
        public class Query : IRequest<Result<Unit>>
        {
            public string UserId { get; set; }
        }
        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.UserId).NotEmpty();
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
                User self = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == this._userAccessor.GetUsername());
                if (self == null) return Result<Unit>.Unauthorize();

                User user = await this._context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
                if (user == null) return null;

                Friend friendship = await this._context.Friends.Where(f => (f.Receiver == self && f.Sender == user) || (f.Sender == self && f.Receiver == user)).FirstOrDefaultAsync();
                if (friendship == null) return Result<Unit>.Failure("Users don't have a friend request!");
                if (friendship.Receiver != self && friendship.Sender != self) return Result<Unit>.Unauthorize();

                this._context.Friends.Remove(friendship);

                bool success = await this._context.SaveChangesAsync() > 0;
                if (!success) return Result<Unit>.Failure("Failed to remove friend!");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
