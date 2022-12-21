using Application.Core;
using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class UpdateMovie
    {
        public class Query : IRequest<Result<Unit>>
        {
            public MovieResponse NewMovie { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Unit>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<Unit>> Handle(Query request, CancellationToken cancellationToken)
            {
                Movie movie = this._context.Movies.FirstOrDefault(m => m.Id == request.NewMovie.movie.Id);
                if (movie == null) return null;

                movie.CoverUrl = request.NewMovie.movie.CoverUrl;
                movie.Name = request.NewMovie.movie.Name;
                movie.ReleaseDate = request.NewMovie.movie.ReleaseDate;

                bool success = await _context.SaveChangesAsync() > 0;

                if (!success) return Result<Unit>.Failure("Failed to update the movie.");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
