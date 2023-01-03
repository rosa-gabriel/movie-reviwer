using Domain;
using FluentValidation;

namespace Application.Validators
{
    public class TagNameValidator : AbstractValidator<Tag>
    {
        public TagNameValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}