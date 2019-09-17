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
