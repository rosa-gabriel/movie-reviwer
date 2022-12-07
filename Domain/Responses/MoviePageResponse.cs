using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Responses
{
    public class MoviePageResponse
    {
        public List<Movie> movies {get; set;}
        public int count {get; set;}
    }
}