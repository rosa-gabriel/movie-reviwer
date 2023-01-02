using Domain;
using FluentValidation;

namespace Application.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Bio).MaximumLength(200);
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.ProfileImageUrl).NotEmpty();
            RuleFor(x => x.UserName).NotEmpty();
        }
    }
}