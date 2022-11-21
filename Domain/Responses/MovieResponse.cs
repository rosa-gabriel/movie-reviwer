using System.Text.Json.Serialization;

namespace Domain
{
    public class MovieResponse
    {
        public Movie movie { get; set; }
        public int favorites { get; set; }
        public List<TagResponse> tags { get; set; }
        public List<CastResponse> castMembers { get; set; }
    }
}