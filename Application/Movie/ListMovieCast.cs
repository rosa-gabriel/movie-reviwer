using Application.Core;
using Application.Validators;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListMovieCast
    {
        public class Query : IRequest<Result<List<CastView>>>
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

        public class Handler : IRequestHandler<Query, Result<List<CastView>>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<List<CastView>>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<CastRole> cast = await _context.CastRoles.Include(ce => ce.Person).Where(ce => ce.Movie == request.movie).ToListAsync();
                List<CastView> castView = new List<CastView>();
                foreach (CastRole cv in cast)
                {
                    castView.Add(new CastView
                    {
                        PersonId = cv.Person.Id,
                        Name = cv.Person.Name,
                        Role = cv.Role
                    });
                }
                return Result<List<CastView>>.Success(castView);
            }
        }
    }
}
