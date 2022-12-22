using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class User : IdentityUser
    {
        public string ProfileImageUrl { get; set; }
        public string Bio { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public bool IsAdmin { get; set; }

        public UserView ToUserView()
        {
            UserView uv = new UserView();
            uv.Id = Id;
            uv.Name = UserName;
            uv.ProfileImageUrl = ProfileImageUrl;

            return uv;
        }
    }
}