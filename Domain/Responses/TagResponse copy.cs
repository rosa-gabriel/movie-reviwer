using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class CastResponse
    {
        public CastResponse(int id, string name, string role)
        {
            this.PersonId = id;
            this.Name = name;
            this.Role = role;
        }
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}