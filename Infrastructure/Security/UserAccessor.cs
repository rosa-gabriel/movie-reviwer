using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security
{
    public class UserAccessor : IUserAccessor
    {
        public readonly IHttpContextAccessor _iHttpContextAccessor;
        public UserAccessor(IHttpContextAccessor iHttpContextAccessor)
        {
            this._iHttpContextAccessor = iHttpContextAccessor;
        }
        public string GetUsername()
        {
            return _iHttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }
    }
}