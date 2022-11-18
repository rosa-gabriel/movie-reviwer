using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class TagResponse
    {
        public TagResponse(int id, string name, int entries)
        {
            this.TagId = id;
            this.Name = name;
            this.Entries = entries;
        }
        public int TagId { get; set; }
        public string Name { get; set; }
        public int Entries { get; set; }
    }
}