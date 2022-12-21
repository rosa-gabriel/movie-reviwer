using Application.Core;
using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class AddMovie
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

                bool success = await _context.SaveChangesAsync() > 0;

                if(!success) return Result<Unit>.Failure("Failed to add Movie.");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
