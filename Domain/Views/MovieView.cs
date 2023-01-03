using System.Text.Json.Serialization;

namespace Domain
{
    public class MovieView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CoverUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}