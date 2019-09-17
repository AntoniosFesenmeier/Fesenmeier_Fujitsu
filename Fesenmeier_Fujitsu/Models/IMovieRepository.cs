using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fesenmeier_Fujitsu.Models
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetAllMovies(string searchString);

        Task<Movie> GetMovie(string id);

        void Create(Movie movie);

        Task<bool> Delete(string id);

        Task<Movie> Update(string id, Movie movie);
    }
}
