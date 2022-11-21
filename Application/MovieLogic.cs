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
        private async Task<int> newTagId()
        {
            Random rnd = new Random();
            int id = 0;
            TagName? dataTag = null;
            do
            {
                id = rnd.Next(111111, 999999);
                dataTag = await _context.TagNames.FindAsync(id);
            } while (dataTag != null);
            return id;
        }
        private async Task<int> newMovieId()
        {
            Random rnd = new Random();
            int id = 0;
            Movie? dataMovie = null;
            do
            {
                id = rnd.Next(111111, 999999);
                dataMovie = await _context.Movies.FindAsync(id);
            } while (dataMovie != null);
            return id;
        }
        private async Task<int> newTagEntryId()
        {
            Random rnd = new Random();
            int id = 0;
            TagEntry? dataTag = null;
            do
            {
                id = rnd.Next(111111, 999999);
                dataTag = await _context.TagEntries.FindAsync(id);
            } while (dataTag != null);
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
        //Get all the tags in the database as a list<TagResponse> format
        public async Task<List<TagResponse>> GetAllTags()
        {
            try
            {
                List<TagName> tags = await this._context.TagNames.ToListAsync();
                List<TagResponse> response = new List<TagResponse>();
                foreach (TagName t in tags)
                {
                    TagResponse item = new TagResponse()
                    {
                        TagId = t.Id,
                        Name = t.Name,
                    };
                    List<TagEntry> entries = await this._context.TagEntries.ToListAsync();
                    item.Entries = entries.Count();
                    response.Add(item);
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        //Adds a movie to the database
        public async Task PostMovie(MovieResponse newMovie)
        {
            if (newMovie.movie.Name.Trim().Length == 0 || newMovie.movie.CoverUrl.Trim().Length == 0) throw new Exception();
            try
            {
                newMovie.movie.Id = await this.newMovieId();
                _context.Movies.Add(newMovie.movie);
                foreach (TagResponse tr in newMovie.tags)
                {
                    TagEntry newTagEntry = new TagEntry();
                    newTagEntry.Id = await this.newTagEntryId();
                    newTagEntry.Tag = await this._context.TagNames.FindAsync(tr.TagId);
                    newTagEntry.Film = newMovie.movie;
                    _context.TagEntries.Add(newTagEntry);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task PostTag(TagName newTag)
        {
            if (newTag.Name.Trim().Length == 0 || newTag.Name.Trim().Length == 0) throw new Exception();
            try
            {
                newTag.Id = await this.newTagId();
                _context.TagNames.Add(newTag);
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
                tagResponses.Add(new TagResponse()
                {
                    TagId = t.Id,
                    Name = t.Name,
                    Entries = count
                });
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
                castResponses.Add(new CastResponse()
                {
                    PersonId = c.Film.Id,
                    Name = c.Film.Name,
                    Role = c.Role
                });
            }
            return castResponses;
        }
    }
}