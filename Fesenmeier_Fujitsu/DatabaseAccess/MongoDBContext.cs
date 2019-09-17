using Fesenmeier_Fujitsu.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fesenmeier_Fujitsu.DatabaseAccess
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database = null;
       
        public MongoDBContext(IConfiguration config)
        {
        
            MongoClient dbClient = new MongoClient(config.GetConnectionString("MovieDBConnection"));
            _database = dbClient.GetDatabase(config.GetValue<string>("DatabaseName"));  
        }

        public IMongoCollection<Movie> Movies
        {
            get
            {
                return _database.GetCollection<Movie>("movie");
            }
        }
    }
}
