using Domain;
using FluentValidation;

namespace Application.Validators
{
    public class TagNameValidator : AbstractValidator<TagName>
    {
        public TagNameValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}