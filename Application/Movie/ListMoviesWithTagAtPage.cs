using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListMoviesWithTagAtPage
    {
        public class Query : IRequest<MoviePageResponse>
        {
            public Guid Id { get; set; }
            public int Page{ get; set; }
        }

        public class Handler : IRequestHandler<Query, MoviePageResponse>
        {
            public readonly DataContext _context;

            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<MoviePageResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    List<Movie> response = await _context.TagEntries.Include(m => m.Film).Where(m => m.Tag.Id == request.Id).Select(m => m.Film).OrderByDescending(m => m.ReleaseDate).ToListAsync();
                    return new MoviePageResponse
                    {
                        movies = response,
                        count = (int)Math.Ceiling(((double)this._context.TagEntries.Where(te => te.Tag.Id == request.Id).Count()) / 25),
                    };
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
        }
    }
}