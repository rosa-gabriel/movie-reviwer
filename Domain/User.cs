using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class User : IdentityUser
    {
        public string ProfileImageUrl { get; set; }
        public string Bio { get; set; }
    }
}