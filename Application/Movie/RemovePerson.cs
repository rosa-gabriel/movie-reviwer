using Application.Core;
using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class RemovePerson
    {
        public class Query : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
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
                Person person = await this._context.People.Where(p => p.Id == request.Id).FirstOrDefaultAsync();
                if (person == null) return null;

                IQueryable<CastEntry> cast = this._context.CastEntries.Where(ce => ce.Person == person);
                this._context.CastEntries.RemoveRange(cast);

                this._context.Remove(person);

                bool success = await _context.SaveChangesAsync() > 0;

                if (!success) return Result<Unit>.Failure("Failed to delete the movie.");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
