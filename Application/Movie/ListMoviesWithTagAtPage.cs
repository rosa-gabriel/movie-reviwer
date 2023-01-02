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
        public class Query : IRequest<Result<MoviePageResponse>>
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

        public class Handler : IRequestHandler<Query, Result<MoviePageResponse>>
        {
            public readonly DataContext _context;

            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<MoviePageResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<Movie> response = await _context.TagEntries.Include(m => m.Film).Where(m => m.Tag.Id == request.Id).Select(m => m.Film).OrderByDescending(m => m.ReleaseDate).ToListAsync();
                return Result<MoviePageResponse>.Success(new MoviePageResponse
                {
                    movies = response,
                    count = (int)Math.Ceiling(((double)this._context.TagEntries.Where(te => te.Tag.Id == request.Id).Count()) / 25),
                });
            }
        }
    }
}