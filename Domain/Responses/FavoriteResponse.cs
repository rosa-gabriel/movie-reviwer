using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class FavoriteResponse
    {
        public Guid movieId { get; set; }
        public bool desiredBool { get; set; }
    }
}