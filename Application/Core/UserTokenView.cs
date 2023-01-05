using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.temp
{
    public class UserTokenView
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; }
        public bool Confirmed { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}