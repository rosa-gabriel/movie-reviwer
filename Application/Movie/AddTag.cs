using Application.Core;
using Application.Interfaces;
using Application.Validators;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application
{
    public class AddTag
    {
        public class Query : IRequest<Result<Tag>>
        {
            public Tag NewTag { get; set; }
        }

        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.NewTag).SetValidator(new TagNameValidator());
            }
        }

        public class Handler : IRequestHandler<Query, Result<Tag>>
        {
            public readonly DataContext _context;
            public readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this._context = context;
                this._userAccessor = userAccessor;
            }

            public async Task<Result<Tag>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (!await this._userAccessor.CheckIfCurrentUserIsAdmin()) return Result<Tag>.Unauthorize();

                Tag newTag = new Tag
                {
                    Id = new Guid(),
                    Name = request.NewTag.Name
                };

                this._context.Tags.Add(newTag);
                bool success = await this._context.SaveChangesAsync() > 0;

                if (!success) return Result<Tag>.Failure("Failed to add tag.");

                return Result<Tag>.Success(request.NewTag);
            }
        }
    }
}
