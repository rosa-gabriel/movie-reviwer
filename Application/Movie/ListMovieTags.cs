using Application.Core;
using Application.Validators;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ListMovieTags
    {
        public class Query : IRequest<Result<List<TagView>>>
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
        public class Handler : IRequestHandler<Query, Result<List<TagView>>>
        {
            public readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<List<TagView>>> Handle(Query request, CancellationToken cancellationToken)
            {
                Movie movie = await this._context.Movies.FirstOrDefaultAsync(m => m.Id == request.movie.Id);
                if (movie == null) return null;

                List<Tag> tags = movie.Tags.ToList();
                List<TagView> tagViews = new List<TagView>();
                foreach (Tag t in tags)
                {
                    TagView tv = t.ToTagView();
                    tv.Entries = tags.Count();
                    tagViews.Add(tv);
                }

                return Result<List<TagView>>.Success(tagViews);
            }
        }
    }
}
