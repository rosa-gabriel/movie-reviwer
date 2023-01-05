using Application.Core;
using Domain;
using Domain.Responses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListMoviesSearchAtPage
    {
        public class Query : IRequest<Result<MoviePageView>>
        {
            public string Filter { get; set; }
            public int Page { get; set; }
        }

        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Filter).NotEmpty();
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
                IQueryable<Movie> search = _context.Movies.Where(m => m.Name.ToLower().Contains(request.Filter.ToLower()) || m.Tags.Any(t => t.Name.ToLower().Contains(request.Filter.ToLower())) || m.Cast.Any(c => c.Person.Name.ToLower().Contains(request.Filter)));

                List<Movie> response = await search.OrderByDescending(ce => ce.ReleaseDate).Take(25).Skip((request.Page - 1) * 25).ToListAsync();

                return Result<MoviePageView>.Success(new MoviePageView(
                    response.Take(25).Skip((request.Page - 1) * 25).ToList(),
                    (int)Math.Ceiling(((double)search.Count()) / 25)
                ));
            }
        }
    }
}