using Domain;
using FluentValidation;

namespace Application.Validators
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(x => x.Message).NotEmpty().Length(1, 50);
        }
    }
}