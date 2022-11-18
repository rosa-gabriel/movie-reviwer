using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class FavoriteEntry
    {
        [Key]
        public int Id{ get; set; }
        public Movie Film{ get; set; }
        public User Fan{ get; set; }
    }
}