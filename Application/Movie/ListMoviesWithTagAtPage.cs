using Application.Core;
using Domain;
using Domain.Responses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListMoviesWithTagAtPage
    {
        public class Query : IRequest<Result<MoviePageView>>
        {
            public Guid Id { get; set; }
            public int Page { get; set; }
        }
        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotEmpty();
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
                //List<Movie> movies = (await _context.Tags.Where(t => t.Id == request.Id).FirstOrDefaultAsync()).Movies.OrderByDescending(m => m.ReleaseDate).ToList();
                Tag tag = await _context.Tags.Include(t => t.Movies).FirstOrDefaultAsync(t => t.Id == request.Id);
                if (tag == null) return null;
                List<Movie> movies = tag.Movies.OrderByDescending(m => m.ReleaseDate).ToList();

                return Result<MoviePageView>.Success(new MoviePageView(
                     movies.Take(25).Skip((request.Page - 1) * 25).ToList(),
                     (int)Math.Ceiling(((Double)movies.Count() / 25))
                ));
            }
        }
    }
}