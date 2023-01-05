using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Validators;
using FluentValidation;

namespace Application
{
    public class UpdatePerson
    {
        public class Query : IRequest<Result<Unit>>
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

                Person person = await this._context.People.FirstOrDefaultAsync(p => p.Id == request.NewPerson.Id);
                if (person == null) return null;

                person.Biography = request.NewPerson.Biography;
                person.Name = request.NewPerson.Name;
                person.Birthday = request.NewPerson.Birthday;
                person.Gender = request.NewPerson.Gender;
                person.ProfileImageUrl = request.NewPerson.ProfileImageUrl;

                bool success = await this._context.SaveChangesAsync() > 0;
                if (success) return Result<Unit>.Success(Unit.Value);
                return Result<Unit>.Failure("Failed to update person!");
            }
        }
    }
}
