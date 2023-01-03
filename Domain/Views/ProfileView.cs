using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class ProfileView
    {
        public string Id { get; set; }
        public bool IsLogedIn { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Movie> RecentFavorites { get; set; }
        public bool IsFriend { get; set; }
        public bool HasRequested { get; set; }
    }
}