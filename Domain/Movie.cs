using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Movie
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CoverUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<CastRole> Cast { get; set; } = new List<CastRole>();

        public MovieInfoView ToMovieInfoView()
        {
            MovieInfoView mv = new MovieInfoView();
            mv.Id = this.Id;
            mv.Name = this.Name;
            mv.CoverUrl = this.CoverUrl;
            mv.ReleaseDate = this.ReleaseDate;
            mv.favorites = this.Favorites.Count();
            foreach (Tag t in this.Tags)
            {
                mv.tags.Add(t.ToTagView());
            }
            foreach (CastRole cr in this.Cast)
            {
                mv.castMembers.Add(cr.ToCastView());
            }
            return mv;
        }

        public MovieView ToMovieView()
        {
            MovieView mv = new MovieView();
            mv.Id = this.Id;
            mv.Name = this.Name;
            mv.CoverUrl = this.CoverUrl;
            mv.ReleaseDate = this.ReleaseDate;
            return mv;
        }
    }
}