using MongoDB.Driver;
using Stock.Shared.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Shared.Services.Concretes
{
    public class MongoDbService : IMongoDbService
    {

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            IMongoDatabase db = getDatabase();
            return db.GetCollection<T>(collectionName);
        }

        public IMongoDatabase getDatabase(string databaseName = "StockDB", string connectionString = "mongodb://localhost:27017")
        {
            MongoClient client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        }
    }
}
