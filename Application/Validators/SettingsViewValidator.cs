using Domain.Views;
using FluentValidation;

namespace Application.Validators
{
    public class SettingsViewValidator : AbstractValidator<SettingsView>
    {
        public SettingsViewValidator()
        {
            RuleFor(x => x.Bio).MaximumLength(200);
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.ProfileImageUrl).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
        }
    }
}