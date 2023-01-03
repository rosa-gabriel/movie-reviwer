using Application.Core;
using Domain;
using Domain.Responses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListMoviesWithPersonAtPage
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
                List<Movie> response = await _context.CastRoles.Include(ce => ce.Movie).Where(ce => ce.Person.Id == request.Id).Select(ce => ce.Movie).OrderByDescending(ce => ce.ReleaseDate).Take(25).Skip((request.Page - 1) * 25).ToListAsync();
                return Result<MoviePageView>.Success(new MoviePageView(
                    response.Take(25).Skip((request.Page - 1) * 25).ToList(),
                    (int)Math.Ceiling(((double)this._context.CastRoles.Where(ce => ce.Person.Id == request.Id).Count()) / 25)
                ));
            }
        }
    }
}