using System.Security.Claims;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security
{
    public class UserAccessor : IUserAccessor
    {
        public readonly DataContext _context;
        public readonly IHttpContextAccessor _iHttpContextAccessor;
        public UserAccessor(IHttpContextAccessor iHttpContextAccessor, DataContext context)
        {
            this._iHttpContextAccessor = iHttpContextAccessor;
            this._context = context;
        }
        public string GetUsername()
        {
            return _iHttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }
        public async Task<bool> CheckIfCurrentUserIsAdmin()
        {
            User user = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == this.GetUsername());
            if (user == null) return false;
            return user.IsAdmin;
        }
    }
}