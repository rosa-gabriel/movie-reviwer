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
    public class UpdatePerson
    {
        public class Query : IRequest<Result<Unit>>
        {
            public Person NewPerson { get; set; }
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
