using System.Text.Json.Serialization;

namespace Domain
{
    public class MovieInfoView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CoverUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int favorites { get; set; }
        public bool isFavorite { get; set; }
        public List<TagView> tags { get; set; } = new List<TagView>();
        public List<CastView> castMembers { get; set; } = new List<CastView>();
    }
}