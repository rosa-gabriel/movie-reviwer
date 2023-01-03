using Application.Core;
using Domain;
using Domain.Responses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListMoviesAtPage
    {
        public class Query : IRequest<Result<MoviePageView>>
        {
            public int Page { get; set; }
        }

        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Page).NotEmpty().GreaterThanOrEqualTo(1);
            }
        }

        public class Handler : IRequestHandler<Query, Result<MoviePageView>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<MoviePageView>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<Movie> movies = await this._context.Movies.OrderByDescending(m => m.ReleaseDate).Skip((request.Page - 1) * 25).Take(25).ToListAsync();
                if (movies.Count == 0 && request.Page > 1) return null;

                return Result<MoviePageView>.Success(new MoviePageView(movies.Take(25).Skip((request.Page - 1) * 25).ToList(), (int)Math.Ceiling(((double)this._context.Movies.Count()) / 25)));
            }
        }
    }
}