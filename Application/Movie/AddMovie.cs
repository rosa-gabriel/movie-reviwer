using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class AddMovie
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
                    _context.Movies.Add(request.NewMovie.movie);
                    foreach (TagResponse tr in request.NewMovie.tags)
                    {
                        TagEntry newTagEntry = new TagEntry();
                        newTagEntry.Tag = await this._context.TagNames.FindAsync(tr.TagId);
                        newTagEntry.Film = request.NewMovie.movie;
                        _context.TagEntries.Add(newTagEntry);
                    }

                    foreach (CastResponse cr in request.NewMovie.castMembers)
                    {
                        CastEntry newCastEntry = new CastEntry();
                        newCastEntry.Person = await this._context.People.Where(p => p.Name == cr.Name).FirstAsync();
                        newCastEntry.Role = cr.Role;
                        newCastEntry.Film = request.NewMovie.movie;
                        _context.CastEntries.Add(newCastEntry);
                    }

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
