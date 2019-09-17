using Fesenmeier_Fujitsu.DatabaseAccess;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fesenmeier_Fujitsu.Models
{
    public class MovieRepository : IMovieRepository
    {

        //private List<Movie> _movieList;

        //public MockMovieRepository()
        //{
        //    _movieList = new List<Movie>()
        //    {
        //        new Movie(){ Id = "1", Title = "Apocalypse Now", Director = "Francis Ford Coppola", Actors = "Martin Sheen, Marlon Brando, Robert Duvall", Image = "https://m.media-amazon.com/images/M/MV5BZTNkZmU0ZWQtZjQzMy00YTNmLWFmN2MtZGNkMmU1OThmMGYwXkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_SY1000_CR0,0,652,1000_AL_.jpg", Year = 1979 },
        //        new Movie(){ Id = "2", Title = "A clockwork orange", Director = "Stanley Kubrick", Actors = "Malcolm McDowell, Patrick Magee",  Image = "https://m.media-amazon.com/images/M/MV5BMTY3MjM1Mzc4N15BMl5BanBnXkFtZTgwODM0NzAxMDE@._V1_SY1000_CR0,0,675,1000_AL_.jpg", Year = 1971 }
        //    };
        //}

        //public Movie Add(Movie movie)
        //{
        //    movie.Id = _movieList.Max(m => m.Id) + 1;
        //    _movieList.Add(movie);
        //    return movie;
        //}

        //public void Delete(string Id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Movie Edit(Movie movie)
        //{
        //    var oldMovie = _movieList.FirstOrDefault(m => m.Id == movie.Id);            
        //    if(oldMovie != null)
        //    {
        //        var index = _movieList.IndexOf(oldMovie);
        //        _movieList.RemoveAt(index);
        //        _movieList.Insert(index, movie);
        //    }

        //    return movie;   
        //}

        //public Movie GetMovie(string Id)
        //{
        //    return _movieList.FirstOrDefault(x => x.Id == Id);
        //}

        //public List<Movie> GetMovies()
        //{
        //    return _movieList;
        //}

        private readonly IConfiguration _config;
        private readonly MongoDBContext _context = null;

        public MovieRepository(IConfiguration config)
        {
            _config = config;
            _context = new MongoDBContext(config);
        }

        public async void Create(Movie movie)
        {
            try
            {
                await _context.Movies.InsertOneAsync(movie);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                DeleteResult actionResult
                    = await _context.Movies.DeleteOneAsync(
                        Builders<Movie>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<List<Movie>> GetAllMovies(string searchString = null)
        {
            try
            {
                if (searchString != null)
                {
                    string lowerSearchString = searchString.ToLowerInvariant();
                    int year;
                    bool y = Int32.TryParse(searchString, out year);
                    return await _context.Movies.Find(m => m.title.ToLowerInvariant().Contains(lowerSearchString)
                        || m.actors.ToLowerInvariant().Contains(lowerSearchString)
                        || m.director.ToLowerInvariant().Contains(lowerSearchString)
                        || (y && m.year.Equals(year))
                    ).ToListAsync();

                }

                return await _context.Movies
                        .Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<Movie> GetMovie(string id)
        {
            try
            {
                ObjectId internalId = GetInternalId(id);
                return await _context.Movies.Find(movie => movie.Id == internalId.ToString()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Movie> Update(string id, Movie movie)
        {
            try
            {
                ReplaceOneResult actionResult = await _context.Movies
                    .ReplaceOneAsync(n => n.Id.Equals(id),
                        movie,
                        new UpdateOptions { IsUpsert = true });
                return movie;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ObjectId GetInternalId(string id)
        {
            ObjectId internalId;
            if (!ObjectId.TryParse(id, out internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }
    }
}
