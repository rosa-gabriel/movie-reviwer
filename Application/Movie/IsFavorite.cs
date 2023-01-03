using Application.Core;
using Application.Validators;
using Domain;
using Domain.Responses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class IsFavorite
    {
        public class Query : IRequest<Result<bool>>
        {
            public Guid MovieId { get; set; }
            public User user { get; set; }
        }

        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.MovieId).NotEmpty();
                RuleFor(x => x.user).SetValidator(new UserValidator());
            }
        }

        public class Handler : IRequestHandler<Query, Result<bool>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<bool>> Handle(Query request, CancellationToken cancellationToken)
            {
                Favorite favorite = await _context.Favorites.FirstOrDefaultAsync(fe => fe.Movie.Id == request.MovieId && fe.User == request.user);
                return Result<bool>.Success(favorite != null);
            }
        }
    }
}
