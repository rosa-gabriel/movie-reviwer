using Application.Validators;
using Domain;
using Domain.Responses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class CountMovieFavites
    {
        public class Query : IRequest<int>
        {
            public Movie movie { get; set; }
        }

        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.movie).SetValidator(new MovieValidator());
            }
        }

        public class Handler : IRequestHandler<Query, int>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<int> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request.movie == null) throw new Exception();
                List<Favorite> count = await _context.Favorites.Where(fe => fe.Movie == request.movie).ToListAsync();
                return count.Count();
            }
        }
    }
}
