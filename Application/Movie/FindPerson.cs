using Application.Core;
using Domain;
using Domain.Responses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class FindPerson
    {
        public class Query : IRequest<Result<Person>>
        {
            public Guid Id { get; set; }
        }

        public class QueryValidation : AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, Result<Person>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }
            public async Task<Result<Person>> Handle(Query request, CancellationToken cancellationToken)
            {
                Person teste = await this._context.People.FirstOrDefaultAsync();
                Person person = await this._context.People.FirstOrDefaultAsync(p => p.Id == request.Id);
                if (person == null) return null;

                return Result<Person>.Success(person);
            }
        }
    }
}