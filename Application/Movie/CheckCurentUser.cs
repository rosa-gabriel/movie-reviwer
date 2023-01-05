using Application.Core;
using Application.temp;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application
{
    public class CheckCurentUser
    {
        public class Query : IRequest<Result<UserTokenView>>
        {
            public LoginDto loginDto { get; set; }
        }
        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
            }
        }

        public class Handler : IRequestHandler<Query, Result<UserTokenView>>
        {
            public readonly UserManager<User> _userManager;
            public readonly DataContext _context;
            public readonly IMediator _mediator;
            public readonly IUserAccessor _userAccessor;
            public readonly ITokenService _tokenService;
            public Handler(UserManager<User> userManager, ITokenService tokenService, DataContext context, IMediator mediator, IUserAccessor userAccessor)
            {
                this._userManager = userManager;
                this._context = context;
                this._mediator = mediator;
                this._userAccessor = userAccessor;
                this._tokenService = tokenService;
            }

            public async Task<Result<UserTokenView>> Handle(Query request, CancellationToken cancellationToken)
            {
                User user = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == this._userAccessor.GetUsername());
                if (user == null) return Result<UserTokenView>.Unauthorize();

                UserTokenView utv = new UserTokenView
                {
                    Email = user.Email,
                    Username = user.UserName,
                    ProfileImageUrl = user.ProfileImageUrl,
                    Token = this._tokenService.CreateToken(user),
                    Id = user.Id,
                    IsAdmin = user.IsAdmin,
                    Confirmed = user.EmailConfirmed
                };

                return Result<UserTokenView>.Success(utv);
            }
        }
    }
}
