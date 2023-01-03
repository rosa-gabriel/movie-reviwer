using Application.Core;
using Domain;
using Domain.Responses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class FindTag
    {
        public class Query : IRequest<Result<Tag>>
        {
            public Guid Id { get; set; }
        }

        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, Result<Tag>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<Tag>> Handle(Query request, CancellationToken cancellationToken)
            {
                Tag tag = await this._context.Tags.Where(tn => tn.Id == request.Id).FirstAsync();
                if (tag == null) return null;

                return Result<Tag>.Success(tag);
            }
        }
    }
}