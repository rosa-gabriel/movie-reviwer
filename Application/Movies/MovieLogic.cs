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

        //Get all the movies in the database on a list<Movie> format
        public async Task<List<Movie>> ListMovies()
        {
            try
            {
                return await this._context.Movies.OrderByDescending(m => m.ReleaseDate).ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<Person> FindPerson(Guid id)
        {
            try
            {
                Person? person = await this._context.People.Where(c => c.Id == id).FirstOrDefaultAsync();
                if (person == null) throw new Exception("Id not found");
                return person;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
        public async Task<TagName> FindTag(Guid id)
        {
            try
            {
                TagName tag = await this._context.TagNames.Where(tn => tn.Id == id).FirstAsync();
                if (tag == null) throw new Exception("Id not found");
                return tag;
            }
            catch (Exception)
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
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<List<Movie>> ListMoviesFromTag(Guid id)
        {
            try
            {
                List<Movie> response = await _context.TagEntries.Include(m => m.Film).Where(m => m.Tag.Id == id).Select(m => m.Film).OrderByDescending(m => m.ReleaseDate).ToListAsync();
                return response;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<List<Movie>> ListMoviesFromPerson(Guid id)
        {
            try
            {
                List<Movie> response = await _context.CastEntries.Include(ce => ce.Film).Where(ce => ce.Person.Id == id).Select(ce => ce.Film).OrderByDescending(ce => ce.ReleaseDate).ToListAsync();
                return response;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<List<Person>> ListCast()
        {
            try
            {
                List<Person> response = await this._context.People.ToListAsync();
                return response;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        //Adds a movie to the database
        public async Task AddMovie(MovieResponse newMovie)
        {
            if (string.IsNullOrWhiteSpace(newMovie.movie.Name)) throw new Exception();
            if (string.IsNullOrWhiteSpace(newMovie.movie.CoverUrl)) newMovie.movie.CoverUrl = "";
            try
            {
                _context.Movies.Add(newMovie.movie);
                foreach (TagResponse tr in newMovie.tags)
                {
                    TagEntry newTagEntry = new TagEntry();
                    newTagEntry.Tag = await this._context.TagNames.FindAsync(tr.TagId);
                    newTagEntry.Film = newMovie.movie;
                    _context.TagEntries.Add(newTagEntry);
                }

                foreach (CastResponse cr in newMovie.castMembers)
                {
                    CastEntry newCastEntry = new CastEntry();
                    newCastEntry.Person = await this._context.People.Where(p => p.Name == cr.Name).FirstAsync();
                    newCastEntry.Role = cr.Role;
                    newCastEntry.Film = newMovie.movie;
                    _context.CastEntries.Add(newCastEntry);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<TagName> AddTag(TagName newTag)
        {
            if (string.IsNullOrWhiteSpace(newTag.Name)) throw new Exception();

            try
            {
                _context.TagNames.Add(newTag);
                await _context.SaveChangesAsync();
                return newTag;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<Person> AddPerson(Person newPerson)
        {
            if (string.IsNullOrWhiteSpace(newPerson.Name)) throw new Exception();
            if (string.IsNullOrWhiteSpace(newPerson.ProfileImageUrl)) newPerson.ProfileImageUrl = "";

            try
            {
                _context.People.Add(newPerson);
                await _context.SaveChangesAsync();
                return newPerson;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        //Get the full inofo for a movie, it's tags and its's cast.
        public async Task<MovieResponse> FindMoviesInfo(Guid id)
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
            catch (Exception)
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
                    Name = c.Person.Name,
                    Role = c.Role
                });
            }
            return castResponses;
        }
        public async Task<List<Movie>> ListFavorites(string userId)
        {
            try
            {
                List<Movie> movies = await _context.FavoriteEntries.Include(fe => fe.Film).Where(fe => fe.Fan.Id == userId).OrderByDescending(fe => fe.FavoriteDate).Select(fe => fe.Film).ToListAsync();
                return movies;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Movie>> ListRecentFavorites(User user)
        {
            try
            {
                List<Movie> movies = await _context.FavoriteEntries.Include(fe => fe.Film).Where(fe => fe.Fan == user).OrderByDescending(fe => fe.FavoriteDate).Select(fe => fe.Film).Take(5).ToListAsync();
                return movies;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task FavoriteMovie(User user, Guid movieId, bool boolDesire)
        {
            try
            {
                Movie? movie = await _context.Movies.FindAsync(movieId);
                if (movie == null) throw new Exception("Invalid movie Id");

                bool addFavorite = boolDesire && !(await this.isFavorite(user, movieId));

                if (!addFavorite)
                {
                    FavoriteEntry? favoriteEntry = await _context.FavoriteEntries.Where(fe => fe.Fan == user).Where(fe => fe.Film.Id == movieId).FirstOrDefaultAsync();

                    if (favoriteEntry == null) return;

                    _context.FavoriteEntries.Remove(favoriteEntry);
                }
                else
                {
                    FavoriteEntry newFavoriteEntry = new FavoriteEntry
                    {
                        Id = new Guid(),
                        Film = movie,
                        Fan = user,
                        FavoriteDate = DateTime.Now,
                    };
                    _context.FavoriteEntries.Add(newFavoriteEntry);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Boolean> isFavorite(User user, Guid movieId)
        {
            try
            {
                FavoriteEntry? favoriteEntry = await _context.FavoriteEntries.FirstOrDefaultAsync(fe => fe.Film.Id == movieId && fe.Fan == user);
                return favoriteEntry != null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}