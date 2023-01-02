using Domain;
using FluentValidation;

namespace Application.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(x => x.Biography).MaximumLength(1000);
            RuleFor(x => x.Gender).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ProfileImageUrl).NotEmpty();
        }
    }
}