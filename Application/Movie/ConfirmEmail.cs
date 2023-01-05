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
    public class ConfirmEmail
    {
        public class Query : IRequest<Result<Unit>>
        {
            public string Token { get; set; }
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
            public readonly ITokenService _tokenService;
            public readonly SignInManager<User> _signInManager;
            public readonly DataContext _context;
            public Handler(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager, DataContext context)
            {
                this._userManager = userManager;
                this._tokenService = tokenService;
                this._signInManager = signInManager;
                this._context = context;
            }

            public async Task<Result<Unit>> Handle(Query request, CancellationToken cancellationToken)
            {
                string token = Uri.UnescapeDataString(request.Token);

                User user = await this._context.ConfirmationTokens.Where(ct => ct.Token == token).Include(ct => ct.User).Select(ct => ct.User).FirstOrDefaultAsync();
                if (user == null) return null;

                await _userManager.ConfirmEmailAsync(user, token);

                List<ConfirmationEmailToken> tokens = await this._context.ConfirmationTokens.Where(ct => ct.User == user).ToListAsync();
                this._context.ConfirmationTokens.RemoveRange(tokens);
                bool success = await this._context.SaveChangesAsync() > 0;
                if (success)
                {
                    return Result<Unit>.Success(Unit.Value);
                }
                else
                {
                    return Result<Unit>.Failure("Failed to confirm email.");
                }
            }
        }
    }
}
