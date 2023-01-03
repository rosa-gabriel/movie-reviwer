using System.Text.Json.Serialization;

namespace Domain
{
    public class NewMovieView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CoverUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<NewTag> tags { get; set; } = new List<NewTag>();
        public List<NewCastView> castMembers { get; set; } = new List<NewCastView>();

        public Movie ToMovie()
        {
            return new Movie
            {
                Id = new Guid(),
                Name = this.Name,
                ReleaseDate = this.ReleaseDate,
                CoverUrl = this.CoverUrl,
            };
        }
    }
}