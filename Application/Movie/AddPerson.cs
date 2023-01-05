using Application.Core;
using Application.Interfaces;
using Application.Validators;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application
{
    public class AddPerson
    {
        public class Query : IRequest<Result<Person>>
        {
            public Person NewPerson { get; set; }
        }

        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.NewPerson).SetValidator(new PersonValidator());
            }
        }

        public class Handler : IRequestHandler<Query, Result<Person>>
        {
            public readonly DataContext _context;
            public readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this._context = context;
                this._userAccessor = userAccessor;
            }

            public async Task<Result<Person>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (!await this._userAccessor.CheckIfCurrentUserIsAdmin()) return Result<Person>.Unauthorize();

                request.NewPerson.Id = new Guid();

                _context.People.Add(request.NewPerson);
                bool success = await _context.SaveChangesAsync() > 0;

                if (!success) return Result<Person>.Failure("Failed to add person.");

                return Result<Person>.Success(request.NewPerson);
            }
        }
    }
}
