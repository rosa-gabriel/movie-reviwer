using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class UpdateMovie
    {
        public class Query : IRequest<Unit>
        {
            public MovieResponse NewMovie { get; set; }
        }

        public class Handler : IRequestHandler<Query, Unit>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Unit> Handle(Query request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrWhiteSpace(request.NewMovie.movie.Name)) throw new Exception();
                if (string.IsNullOrWhiteSpace(request.NewMovie.movie.CoverUrl)) request.NewMovie.movie.CoverUrl = "";

                try
                {
                    Movie movie = this._context.Movies.FirstOrDefault(m => m.Id == request.NewMovie.movie.Id);
                    if (movie == null) throw new Exception("Movie not found!");

                    movie.CoverUrl = request.NewMovie.movie.CoverUrl;
                    movie.Name = request.NewMovie.movie.Name;
                    movie.ReleaseDate = request.NewMovie.movie.ReleaseDate;

                    await _context.SaveChangesAsync();

                    return new Unit();
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
        }
    }
}
