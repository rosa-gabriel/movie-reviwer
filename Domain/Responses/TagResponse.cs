using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain
{
    public class TagResponse
    {
        public TagResponse(){}
        public int TagId { get; set; }
        public string Name { get; set; }
        public int Entries { get; set; }
    }
}