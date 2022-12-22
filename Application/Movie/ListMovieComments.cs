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
    public class ListMovieComments
    {
        public class Query : IRequest<Result<List<CommentView>>>
        {
            public Guid MovieId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<CommentView>>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<List<CommentView>>> Handle(Query request, CancellationToken cancellationToken)
            {
                Movie movie = await this._context.Movies.Include(m => m.Comments).ThenInclude(c => c.Creator).FirstOrDefaultAsync(m => m.Id == request.MovieId);
                if (movie == null) return null;

                List<Comment> comments = movie.Comments.OrderByDescending(c => c.PostDate).ToList();
                List<CommentView> commentViews = new List<CommentView>();
                foreach (Comment c in comments)
                {
                    CommentView cv = c.ToCommentView();
                    commentViews.Add(cv);
                }

                return Result<List<CommentView>>.Success(commentViews);
            }
        }
    }
}
