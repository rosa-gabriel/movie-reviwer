using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Responses
{
    public class MoviePageView
    {
        public MoviePageView(List<Movie> movies, int count)
        {
            foreach (Movie m in movies)
            {
                this.Movies.Add(m.ToMovieView());
            }
            this.Count = count;
        }
        public List<MovieView> Movies { get; set; } = new List<MovieView>();
        public int Count { get; set; }
    }
}