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
    public class ListUserComments
    {
        public class Query : IRequest<Result<List<CommentView>>> { }

        public class Handler : IRequestHandler<Query, Result<List<CommentView>>>
        {
            public readonly DataContext _context;
            public readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this._context = context;
                this._userAccessor = userAccessor;
            }

            public async Task<Result<List<CommentView>>> Handle(Query request, CancellationToken cancellationToken)
            {
                User user = await this._context.Users.Include(u => u.Comments).FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());
                if (user == null) return null;

                List<Comment> comments = user.Comments.ToList();
                List<CommentView> commentViews = new List<CommentView>();
                foreach (Comment c in comments)
                {
                    commentViews.Add(c.ToCommentView());
                }

                return Result<List<CommentView>>.Success(commentViews);
            }
        }
    }
}
