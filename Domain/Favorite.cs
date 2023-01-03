using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Favorite
    {
        [Key]
        public Guid Id { get; set; }
        public Movie Movie { get; set; }
        public User User { get; set; }
        public DateTime FavoriteDate { get; set; }
    }
}