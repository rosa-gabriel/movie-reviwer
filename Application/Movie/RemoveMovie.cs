using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class RemoveMovie
    {
        public class Query : IRequest<Unit>
        {
            public Guid Id { get; set; }
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
                try
                {
                    Movie movie = await this._context.Movies.Where(m => m.Id == request.Id).FirstOrDefaultAsync();
                    if (movie == null) throw new Exception("Invalid Movie Id!");

                    IQueryable<TagEntry> tags = this._context.TagEntries.Where(te => te.Film == movie);
                    IQueryable<CastEntry> cast = this._context.CastEntries.Where(ce => ce.Film == movie);
                    IQueryable<FavoriteEntry> favorites = this._context.FavoriteEntries.Where(f => f.Film == movie);

                    this._context.TagEntries.RemoveRange(tags);
                    this._context.CastEntries.RemoveRange(cast);
                    this._context.FavoriteEntries.RemoveRange(favorites);
                    await _context.SaveChangesAsync();

                    this._context.Remove(movie);
                    await _context.SaveChangesAsync();

                    return new Unit();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
    }
}
