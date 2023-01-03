using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class CastRole
    {
        public Guid Id { get; set; }
        public string Role { get; set; }
        public Person Person { get; set; }
        public Movie Movie { get; set; }
        public CastView ToCastView()
        {
            return new CastView
            {
                PersonId = this.Person.Id,
                Name = this.Person.Name,
                Role = this.Role,
            };
        }
    }
}
