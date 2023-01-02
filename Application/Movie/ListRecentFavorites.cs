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
                List<Movie> movies = await _context.FavoriteEntries.Include(fe => fe.Film).Where(fe => fe.Fan == request.user).OrderByDescending(fe => fe.FavoriteDate).Select(fe => fe.Film).Take(5).ToListAsync();
                return Result<List<Movie>>.Success(movies);
            }
        }
    }
}