using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using FluentValidation;
using Application.Validators;

namespace Application
{
    public class UpdateComment
    {
        public class Query : IRequest<Result<Unit>>
        {
            public Comment NewComment { get; set; }
        }
        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.NewComment).SetValidator(new CommentValidator());
            }
        }

        public class Handler : IRequestHandler<Query, Result<Unit>>
        {
            public readonly DataContext _context;
            public readonly IUserAccessor _useAccessor;
            public readonly UserManager<User> _userManager;
            public Handler(DataContext context, IUserAccessor userAccessor, UserManager<User> userManager, ITokenService tokenService)
            {
                this._context = context;
                this._useAccessor = userAccessor;
                this._userManager = userManager;
            }

            public async Task<Result<Unit>> Handle(Query request, CancellationToken cancellationToken)
            {
                User user = await this._userManager.FindByNameAsync(this._useAccessor.GetUsername());
                if (user == null) return Result<Unit>.Unauthorize();

                Comment comment = await this._context.Comments.Where(c => c.Creator == user).FirstOrDefaultAsync(u => u.Id == request.NewComment.Id);
                if (comment == null) return null;

                if (comment.Message == request.NewComment.Message) return Result<Unit>.Success(Unit.Value);

                comment.wasEdited = true;
                comment.Message = request.NewComment.Message;

                bool success = await this._context.SaveChangesAsync() > 0;

                if (!success) return Result<Unit>.Failure("Failed to update comment!");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
