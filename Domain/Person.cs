using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Person
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime Birthday { get; set; }
        public string Biography { get; set; }
        public int Gender { get; set; }
    }
}