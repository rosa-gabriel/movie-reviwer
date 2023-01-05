using Application.Core;
using Application.temp;
using Application.Interfaces;
using Domain;
using Domain.Views;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using FluentValidation;
using Application.Validators;

namespace Application
{
    public class Login
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
            public readonly ITokenService _tokenService;
            public readonly SignInManager<User> _signInManager;
            public Handler(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager)
            {
                this._userManager = userManager;
                this._tokenService = tokenService;
                this._signInManager = signInManager;
            }

            public async Task<Result<UserTokenView>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await this._userManager.FindByEmailAsync(request.loginDto.login);
                if (user == null)
                {
                    user = await this._userManager.FindByNameAsync(request.loginDto.login);
                    if (user == null) return Result<UserTokenView>.Unauthorize();
                }

                var result = await this._signInManager.CheckPasswordSignInAsync(user, request.loginDto.Password, false);

                if (result.Succeeded)
                {
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
                return Result<UserTokenView>.Unauthorize();
            }
        }
    }
}
