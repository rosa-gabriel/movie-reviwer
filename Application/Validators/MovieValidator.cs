using Domain;
using FluentValidation;

namespace Application.Validators
{
    public class MovieValidator : AbstractValidator<Movie>
    {
        public MovieValidator()
        {
            RuleFor(x => x.CoverUrl).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}