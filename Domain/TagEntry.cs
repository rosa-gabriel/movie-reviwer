using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class TagEntry 
    {
        
        [Key]
        public Guid Id {get; set;}
        public TagName Tag { get; set; }
        public Movie Film{get; set;}
    }
}