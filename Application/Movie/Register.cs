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
    public class Register
    {
        public class Query : IRequest<Result<UserTokenView>>
        {
            public RegisterDto registerDto { get; set; }
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
            public readonly DataContext _context;
            public readonly IEmailService _emailService;
            public Handler(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager, DataContext context, IEmailService emailService)
            {
                this._userManager = userManager;
                this._tokenService = tokenService;
                this._signInManager = signInManager;
                this._context = context;
                this._emailService = emailService;
            }

            public async Task<Result<UserTokenView>> Handle(Query request, CancellationToken cancellationToken)
            {
                bool usernameTaken = await _userManager.Users.AnyAsync(u => u.UserName == request.registerDto.UserName);
                User userCheck = await this._context.Users.FirstOrDefaultAsync(u => u.Email == request.registerDto.Email);

                if (userCheck != null && userCheck.EmailConfirmed) return Result<UserTokenView>.Failure("Email already taken!");
                else if (userCheck != null && !userCheck.EmailConfirmed) await _userManager.DeleteAsync(userCheck);

                else if ((userCheck != null && userCheck.NormalizedEmail != request.registerDto.UserName.ToUpper() && usernameTaken) || (userCheck == null && usernameTaken)) return Result<UserTokenView>.Failure("Username already taken!");

                User user = new User
                {
                    UserName = request.registerDto.UserName,
                    Email = request.registerDto.Email,
                    ProfileImageUrl = "https://i.imgur.com/mggfJH8.png",
                    CreationDate = DateTime.Now,
                    Bio = "",
                    IsAdmin = false
                };

                var response = await _userManager.CreateAsync(user, request.registerDto.Password);


                if (response.Succeeded)
                {
                    UserTokenView utv = new UserTokenView
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Username = user.UserName,
                        ProfileImageUrl = user.ProfileImageUrl,
                        Token = _tokenService.CreateToken(user),
                        IsAdmin = user.IsAdmin,
                        Confirmed = user.EmailConfirmed
                    };

                    string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    this._context.ConfirmationTokens.Add(new ConfirmationEmailToken { Token = token, User = user });
                    await this._context.SaveChangesAsync();

                    string encodedToken = Uri.EscapeDataString(token);

                    this._emailService.Send(new Email { To = user.Email, Subjetct = "Confirm email - MovieApp", Body = "Confirm your email at: https://gabrielwaif.github.io/MovieApp/#/account/confirm/" + encodedToken });

                    return Result<UserTokenView>.Success(utv);
                }
                return Result<UserTokenView>.Failure("Failed ");
            }
        }
    }
}
