using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
        public TagView ToTagView()
        {
            return new TagView
            {
                TagId = this.Id,
                Name = this.Name,
                Entries = this.Movies.Count()
            };
        }

    }
}