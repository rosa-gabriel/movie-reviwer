using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class CastEntry
    {
        [Key]
        public int Id {get; set;}
        public string Role { get; set; }
        public Cast Person { get; set; }
        public CastType Type {get; set;}
        public Movie Film {get; set;}
    }
}