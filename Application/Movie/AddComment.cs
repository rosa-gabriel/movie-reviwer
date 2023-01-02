using Application.Core;
using Application.Interfaces;
using Application.Validators;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application
{
    public class AddComment
    {
        public class Query : IRequest<Result<Unit>>
        {
            public Comment Comment { get; set; }
            public Guid movieId { get; set; }
        }

        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Comment).SetValidator(new CommentValidator());
                RuleFor(x => x.movieId).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, Result<Unit>>
        {
            public readonly DataContext _context;
            public readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this._context = context;
                this._userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Query request, CancellationToken cancellationToken)
            {
                User user = this._context.Users.FirstOrDefault(u => u.UserName == _userAccessor.GetUsername());
                Movie movie = this._context.Movies.FirstOrDefault(m => m.Id == request.movieId);

                if (user == null) return Result<Unit>.Unauthorize();
                if (movie == null) return null;

                request.Comment.Id = new Guid();
                request.Comment.wasEdited = false;
                request.Comment.PostDate = DateTime.Now;
                request.Comment.Creator = user;
                request.Comment.Movie = movie;

                this._context.Comments.Add(request.Comment);
                await this._context.SaveChangesAsync();

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
