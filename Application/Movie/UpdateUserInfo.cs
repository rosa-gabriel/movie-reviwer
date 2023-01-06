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
    public class UpdateUserInfo
    {
        public class Query : IRequest<Result<UserTokenView>>
        {
            public SettingsView NewSettings { get; set; }
        }
        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.NewSettings).SetValidator(new SettingsViewValidator());
            }
        }

        public class Handler : IRequestHandler<Query, Result<UserTokenView>>
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

            public async Task<Result<UserTokenView>> Handle(Query request, CancellationToken cancellationToken)
            {
                User user = await this._userManager.FindByNameAsync(this._useAccessor.GetUsername());
                if (user == null) return Result<UserTokenView>.Unauthorize();

                bool hasChanged = false;

                if ((await this._context.Users.FirstOrDefaultAsync(u => u.UserName == request.NewSettings.Username)) != null && request.NewSettings.Username != user.UserName) return Result<UserTokenView>.Failure("Username already taken! Try another one.");
                if ((await this._context.Users.FirstOrDefaultAsync(u => u.Email == request.NewSettings.Email)) != null && request.NewSettings.Email != user.Email) return Result<UserTokenView>.Failure("Email already taken! Try another one.");

                if (user.Bio != request.NewSettings.Bio)
                {
                    hasChanged = true;
                    user.Bio = request.NewSettings.Bio;
                }

                if (user.ProfileImageUrl != request.NewSettings.ProfileImageUrl)
                {
                    hasChanged = true;
                    user.ProfileImageUrl = request.NewSettings.ProfileImageUrl;
                }

                if (user.UserName != request.NewSettings.Username)
                {
                    hasChanged = true;
                    user.UserName = request.NewSettings.Username;
                    user.NormalizedUserName = request.NewSettings.Username.ToUpper();
                }

                bool success = true;
                if (hasChanged) success = await this._context.SaveChangesAsync() > 0;
                if (!success) return Result<UserTokenView>.Failure("Failed to update user info!");

                if (user.Email != request.NewSettings.Email)
                {
                    var emailToken = await this._userManager.GenerateChangeEmailTokenAsync(user, request.NewSettings.Email);
                    await this._userManager.ChangeEmailAsync(user, request.NewSettings.Email, emailToken);
                }

                if (!String.IsNullOrWhiteSpace(request.NewSettings.NewPassword) && !String.IsNullOrWhiteSpace(request.NewSettings.OldPassword))
                {
                    if (!(await this._userManager.CheckPasswordAsync(user, request.NewSettings.OldPassword))) return Result<UserTokenView>.Failure("Password does't match! Try again.");

                    var passwordToken = await this._userManager.GeneratePasswordResetTokenAsync(user);
                    await this._userManager.ResetPasswordAsync(user, passwordToken, request.NewSettings.NewPassword);
                }

                await this._userManager.UpdateAsync(user);


                return Result<UserTokenView>.Success(new UserTokenView
                {
                    Email = user.Email,
                    Username = user.UserName,
                    ProfileImageUrl = user.ProfileImageUrl,
                    Token = _tokenService.CreateToken(user),
                    Id = user.Id,
                    IsAdmin = user.IsAdmin,
                    Confirmed = user.EmailConfirmed,
                }
                );
            }
        }
    }
}
