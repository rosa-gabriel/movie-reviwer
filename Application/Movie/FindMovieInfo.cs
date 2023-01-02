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
        public class Query : IRequest<Result<MovieResponse>>
        {
            public Guid Id { get; set; }
            public IMediator Mediator { get; set; }
        }

        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, Result<MovieResponse>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<MovieResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                Movie dataMovie = await _context.Movies.FindAsync(request.Id);
                if (dataMovie == null) return null;

                MovieResponse movieResponse = new MovieResponse();
                movieResponse.movie = dataMovie;
                movieResponse.favorites = (await request.Mediator.Send(new CountMovieFavites.Query { movie = dataMovie }));
                movieResponse.tags = (await request.Mediator.Send(new ListMovieTags.Query { movie = dataMovie })).Value;
                movieResponse.castMembers = (await request.Mediator.Send(new ListMovieCast.Query { movie = dataMovie })).Value;

                return Result<MovieResponse>.Success(movieResponse);
            }
        }
    }
}