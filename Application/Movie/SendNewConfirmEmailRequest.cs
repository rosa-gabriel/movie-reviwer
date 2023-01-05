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
    public class SendNewEmailConfirmRequest
    {
        public class Query : IRequest<Result<Unit>>
        {
        }
        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
            }
        }

        public class Handler : IRequestHandler<Query, Result<Unit>>
        {
            public readonly UserManager<User> _userManager;
            public readonly IUserAccessor _userAccessor;
            public readonly ITokenService _tokenService;
            public readonly SignInManager<User> _signInManager;
            public readonly DataContext _context;
            public readonly IEmailService _emailService;
            public Handler(UserManager<User> userManager, IUserAccessor userAccessor, ITokenService tokenService, SignInManager<User> signInManager, DataContext context, IEmailService emailService)
            {
                this._userManager = userManager;
                this._tokenService = tokenService;
                this._userAccessor = userAccessor;
                this._signInManager = signInManager;
                this._context = context;
                this._emailService = emailService;
            }

            public async Task<Result<Unit>> Handle(Query request, CancellationToken cancellationToken)
            {
                User user = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == this._userAccessor.GetUsername());
                if (user == null) return Result<Unit>.Unauthorize();

                if (user.EmailConfirmed) return Result<Unit>.Failure("User alrady has a confirmed email!");

                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                this._context.ConfirmationTokens.Add(new ConfirmationEmailToken { Token = token, User = user });
                await this._context.SaveChangesAsync();

                string encodedToken = Uri.EscapeDataString(token);

                this._emailService.Send(new Email { To = user.Email, Subjetct = "Confirm email - MovieApp", Body = "Confirm your email at: http://localhost:3000/account/confirm/" + encodedToken });

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
