using Application.Core;
using Application.temp;
using Application.Interfaces;
using Domain;
using Domain.Views;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class RemoveComment
    {
        public class Query : IRequest<Result<Unit>>
        {
            public Comment NewComment { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Unit>>
        {
            public readonly DataContext _context;
            public readonly IUserAccessor _useAccessor;
            public readonly UserManager<User> _userManager;
            public readonly ITokenService _tokenService;
            public Handler(DataContext context, IUserAccessor userAccessor, UserManager<User> userManager, ITokenService tokenService)
            {
                this._context = context;
                this._useAccessor = userAccessor;
                this._userManager = userManager;
                this._tokenService = tokenService;
            }

            public async Task<Result<Unit>> Handle(Query request, CancellationToken cancellationToken)
            {
                User user = await this._userManager.FindByNameAsync(this._useAccessor.GetUsername());
                if (user == null) return Result<Unit>.Failure("Invalid user! Try reloging in.");

                Comment comment = await this._context.Comments.Where(c => c.Creator == user).FirstOrDefaultAsync(u => u.Id == request.NewComment.Id);
                if (comment == null) return null;

                this._context.Comments.Remove(comment);

                bool success = await this._context.SaveChangesAsync() > 0;

                if (!success) return Result<Unit>.Failure("Failed to remove comment!");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
