using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Movie 
    {
        [Key]
        public Guid Id{get; set;}
        public string Name {get; set;}
        public string CoverUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}