using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class AddTag
    {
        public class Query : IRequest<Unit>
        {
            public TagName NewTag { get; set; }
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
                if (string.IsNullOrWhiteSpace(request.NewTag.Name)) throw new Exception();

                try
                {
                    _context.TagNames.Add(request.NewTag);
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
