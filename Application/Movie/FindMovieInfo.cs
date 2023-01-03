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
    public class FindMovieInfo
    {
        public class Query : IRequest<Result<MovieInfoView>>
        {
            public Guid Id { get; set; }
        }

        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, Result<MovieInfoView>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<MovieInfoView>> Handle(Query request, CancellationToken cancellationToken)
            {
                Movie movie = await _context.Movies.Include(m => m.Cast).ThenInclude(c => c.Person).Include(m => m.Tags).Include(m => m.Favorites).FirstOrDefaultAsync(m => m.Id == request.Id);
                if (movie == null) return null;

                MovieInfoView movieView = movie.ToMovieInfoView();

                return Result<MovieInfoView>.Success(movieView);
            }
        }
    }
}