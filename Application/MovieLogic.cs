using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application
{
    public class MovieLogic
    {
        private readonly DataContext _context;
        public MovieLogic(DataContext context)
        {
            _context = context;
        }

        //Generates a new unique id
        private async Task<int> newId()
        {
            Random rnd = new Random();
            int id = 0;
            Movie? dataMovie = null;
            while (dataMovie != null)
            {
                id = rnd.Next(111111, 999999);
                dataMovie = await _context.Movies.FindAsync(id);
            }
            return id;
        }

        //Get all the movies in the database on a list<Movie> format
        public async Task<List<Movie>> GetAllMovies()
        {
            try
            {
                return await this._context.Movies.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        //Adds a movie to the database
        public async Task PostMovie(Movie newMovie)
        {
            newMovie.Id = await this.newId();
            if (newMovie.Name.Trim().Length == 0 || newMovie.CoverUrl.Trim().Length == 0) throw new Exception();
            try
            {
                _context.Movies.Add(newMovie);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        //Get the full inofo for a movie, it's tags and its's cast.
        public async Task<MovieResponse> getAllMovieInfo(int id)
        {
            Movie? dataMovie = await _context.Movies.FindAsync(id);
            if (dataMovie == null) throw new Exception();

            try
            {
                MovieResponse movieResponse = new MovieResponse();
                movieResponse.movie = dataMovie;
                movieResponse.favorites = await this.getMovieFavorites(dataMovie);
                movieResponse.tags = await this.getMovieTags(dataMovie);
                movieResponse.castMembers = await this.getMovieCast(dataMovie);
                return movieResponse;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        //Get an int of the favorites of a movie
        public async Task<int> getMovieFavorites(Movie movie)
        {
            if (movie == null) throw new Exception();
            List<FavoriteEntry> count = await _context.FavoriteEntries.Where(fe => fe.Film == movie).ToListAsync();
            return count.Count();
        }
        public async Task<List<TagResponse>> getMovieTags(Movie movie)
        {
            if (movie == null) throw new Exception();
            List<TagName> tags = await _context.TagEntries.Include(fe => fe.Tag).Where(fe => fe.Film == movie).Select(fe => fe.Tag).ToListAsync();
            List<TagResponse> tagResponses = new List<TagResponse>();
            foreach (TagName t in tags)
            {
                var count = (await _context.TagEntries.Where(te => te.Tag == t).ToListAsync()).Count();
                tagResponses.Add(new TagResponse(t.Id, t.Name, count));
            }
            return tagResponses;
        }
        //Get the cast info and role for a movie
        public async Task<List<CastResponse>> getMovieCast(Movie movie)
        {
            if (movie == null) throw new Exception();
            List<CastEntry> cast = await _context.CastEntries.Include(ce => ce.Person).Where(ce => ce.Film == movie).ToListAsync();
            List<CastResponse> castResponses = new List<CastResponse>();
            foreach (CastEntry c in cast)
            {
                castResponses.Add(new CastResponse(c.Film.Id, c.Film.Name, c.Role));
            }
            return castResponses;
        }
    }
}