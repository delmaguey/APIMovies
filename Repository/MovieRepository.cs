using APIMovies.Data;
using APIMovies.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMovies.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _db;

        public MovieRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateMovie(Movie movie)
        {
            _db.Movie.Add(movie);
            return Save();
        }

        public bool DeleteMovie(Movie movie)
        {
            _db.Movie.Remove(movie);
            return Save();
        }

        public bool ExistMovie(string name)
        {
            bool value = _db.Movie.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool ExistMovie(int movieId)
        {
            return _db.Movie.Any(c => c.Id == movieId);
        }

        public Movie GetMovie(int movieId)
        {
            return _db.Movie.FirstOrDefault(c => c.Id == movieId);
        }

        public ICollection<Movie> GetMovies()
        {
            return _db.Movie.OrderBy(x => x.Name).ToList();
        }

        public ICollection<Movie> GetMoviesInCategory(int categoryId)
        {
            return _db.Movie.Include(ca => ca.Category).Where(c => c.CategoryId == categoryId).ToList();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public IEnumerable<Movie> SearchMovie(string name)
        {
            IQueryable<Movie> query = _db.Movie;

            if(!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Name.Contains(name) || e.Description.Contains(name));
            }

            return query.ToList();
        }

        public bool UpdateMovie(Movie movie)
        {
            _db.Movie.Update(movie);
            return Save();
        }
    }
}
