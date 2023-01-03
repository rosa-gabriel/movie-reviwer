using Application.Core;
using Application.Validators;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListRecentFavorites
    {
        public class Query : IRequest<Result<List<Movie>>>
        {
            public User user { get; set; }
        }
        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.user).SetValidator(new UserValidator());
            }
        }

        public class Handler : IRequestHandler<Query, Result<List<Movie>>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<List<Movie>>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<Movie> movies = await _context.Favorites.Include(fe => fe.Movie).Where(fe => fe.User == request.user).OrderByDescending(fe => fe.FavoriteDate).Select(fe => fe.Movie).Take(5).ToListAsync();
                return Result<List<Movie>>.Success(movies);
            }
        }
    }
}