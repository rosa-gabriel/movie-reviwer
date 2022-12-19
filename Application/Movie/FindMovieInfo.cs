using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class FindMovieInfo
    {
        public class Query : IRequest<MovieResponse>
        {
            public Guid Id { get; set; }
            public IMediator Mediator { get; set; }
        }

        public class Handler : IRequestHandler<Query, MovieResponse>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<MovieResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                Movie dataMovie = await _context.Movies.FindAsync(request.Id);
                if (dataMovie == null) throw new Exception();

                try
                {
                    MovieResponse movieResponse = new MovieResponse();
                    movieResponse.movie = dataMovie;
                    movieResponse.favorites = await request.Mediator.Send(new CountMovieFavites.Query { movie = dataMovie });
                    movieResponse.tags = await request.Mediator.Send(new ListMovieTags.Query { movie = dataMovie });
                    movieResponse.castMembers = await request.Mediator.Send(new ListMovieCast.Query { movie = dataMovie });
                    return movieResponse;
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
        }
    }
}