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
        private readonly GenerateIds generateIds;
        public MovieLogic(DataContext context)
        {
            _context = context;
            this.generateIds = new GenerateIds(this._context);
        }

        //Get all the movies in the database on a list<Movie> format
        public async Task<List<Movie>> ListMovies()
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

        public async Task<Cast> FindPerson(int id)
        {
            try
            {
                return await this._context.Casts.Where(c => c.Id == id).FirstAsync();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public async Task<TagName> FindTag(int id)
        {
            try
            {
                return await this._context.TagNames.Where(tn => tn.Id == id).FirstAsync();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        //Get all the tags in the database as a list<TagResponse> format
        public async Task<List<TagResponse>> ListTags()
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

        public async Task<List<Movie>> ListMoviesFromTag(int id)
        {
            try
            {
                List<Movie> response = await _context.TagEntries.Include(m => m.Film).Where(m => m.Tag.Id == id).Select(m => m.Film).ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<List<Cast>> ListCast()
        {
            try
            {
                List<Cast> response = await this._context.Casts.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        //Adds a movie to the database
        public async Task AddMovie(MovieResponse newMovie)
        {
            if (newMovie.movie.Name.Trim().Length == 0 || newMovie.movie.CoverUrl.Trim().Length == 0) throw new Exception();
            try
            {
                newMovie.movie.Id = await this.generateIds.newMovieId();
                _context.Movies.Add(newMovie.movie);
                foreach (TagResponse tr in newMovie.tags)
                {
                    TagEntry newTagEntry = new TagEntry();
                    newTagEntry.Id = await generateIds.newTagEntryId();
                    newTagEntry.Tag = await this._context.TagNames.FindAsync(tr.TagId);
                    newTagEntry.Film = newMovie.movie;
                    _context.TagEntries.Add(newTagEntry);
                }

                foreach (CastResponse cr in newMovie.castMembers)
                {
                    CastEntry newCastEntry = new CastEntry();
                    newCastEntry.Id = await generateIds.newCastEntryId();
                    newCastEntry.Person = await this._context.Casts.Where(p => p.Name == cr.Name).FirstAsync();
                    newCastEntry.Role = cr.Role;
                    newCastEntry.Film = newMovie.movie;
                    _context.CastEntries.Add(newCastEntry);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task AddTag(TagName newTag)
        {
            if (newTag.Name.Trim().Length == 0 || newTag.Name.Trim().Length == 0) throw new Exception();
            try
            {
                newTag.Id = await this.generateIds.newTagId();
                _context.TagNames.Add(newTag);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task AddPerson(Cast newPerson)
        {
            if (newPerson.Name.Trim().Length == 0 || newPerson.ProfileImageUrl.Trim().Length == 0) throw new Exception();
            try
            {
                newPerson.Id = await this.generateIds.newCastId();
                _context.Casts.Add(newPerson);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        //Get the full inofo for a movie, it's tags and its's cast.
        public async Task<MovieResponse> ListMoviesInfo(int id)
        {
            Movie? dataMovie = await _context.Movies.FindAsync(id);
            if (dataMovie == null) throw new Exception();

            try
            {
                MovieResponse movieResponse = new MovieResponse();
                movieResponse.movie = dataMovie;
                movieResponse.favorites = await this.CountMovieFavites(dataMovie);
                movieResponse.tags = await this.ListMovieTags(dataMovie);
                movieResponse.castMembers = await this.ListMovieCast(dataMovie);
                return movieResponse;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        //Get an int of the favorites of a movie
        public async Task<int> CountMovieFavites(Movie movie)
        {
            if (movie == null) throw new Exception();
            List<FavoriteEntry> count = await _context.FavoriteEntries.Where(fe => fe.Film == movie).ToListAsync();
            return count.Count();
        }

        public async Task<List<TagResponse>> ListMovieTags(Movie movie)
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
        public async Task<List<CastResponse>> ListMovieCast(Movie movie)
        {
            if (movie == null) throw new Exception();
            List<CastEntry> cast = await _context.CastEntries.Include(ce => ce.Person).Where(ce => ce.Film == movie).ToListAsync();
            List<CastResponse> castResponses = new List<CastResponse>();
            foreach (CastEntry c in cast)
            {
                castResponses.Add(new CastResponse()
                {
                    PersonId = c.Person.Id,
                    Name = c.Film.Name,
                    Role = c.Role
                });
            }
            return castResponses;
        }
    }
}