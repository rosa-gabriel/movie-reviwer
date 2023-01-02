using Application.Core;
using Application.Interfaces;
using Domain;
using Domain.Responses;
using Domain.Views;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class FindUserSettings
    {
        public class Query : IRequest<Result<SettingsView>> { }

        public class Handler : IRequestHandler<Query, Result<SettingsView>>
        {
            public readonly DataContext _context;
            public readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this._context = context;
                this._userAccessor = userAccessor;
            }
            public async Task<Result<SettingsView>> Handle(Query request, CancellationToken cancellationToken)
            {
                User user = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == this._userAccessor.GetUsername());
                if (user == null) return Result<SettingsView>.Unauthorize();

                SettingsView settings = user.ToSettingsView();

                return Result<SettingsView>.Success(settings);
            }
        }
    }
}