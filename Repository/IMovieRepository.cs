using APIMovies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMovies.Repository
{
    public interface IMovieRepository
    {
        ICollection<Movie> GetMovies();

        ICollection<Movie> GetMoviesInCategory(int categoryId);

        Movie GetMovie(int movieId);

        bool ExistMovie(string name);

        IEnumerable<Movie> SearchMovie(string name);

        bool ExistMovie(int movieId);

        bool CreateMovie(Movie movie);

        bool UpdateMovie(Movie movie);

        bool DeleteMovie(Movie movie);

        bool Save();
    }
}
