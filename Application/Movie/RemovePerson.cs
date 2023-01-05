using Application.Core;
using Application.Interfaces;
using Domain;
using Domain.Responses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class RemovePerson
    {
        public class Query : IRequest<Result<Unit>>
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
                if (!await this._userAccessor.CheckIfCurrentUserIsAdmin()) return Result<Unit>.Unauthorize();

                Person person = await this._context.People.Where(p => p.Id == request.Id).FirstOrDefaultAsync();
                if (person == null) return null;

                IQueryable<CastRole> cast = this._context.CastRoles.Where(ce => ce.Person == person);
                this._context.CastRoles.RemoveRange(cast);

                this._context.Remove(person);

                bool success = await _context.SaveChangesAsync() > 0;

                if (!success) return Result<Unit>.Failure("Failed to delete the movie.");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
