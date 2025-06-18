using KnjizevneKritikeApp.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnjizevneKritikeApp.Services
{
    public class MongoDbService
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public MongoDbService(string connectionString, string databaseName)
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);
        }

        public IMongoDatabase Database => _database;

        public IMongoCollection<Korisnik> Korisnici => _database.GetCollection<Korisnik>("Korisnici");
        public IMongoCollection<Recenzija> Recenzije => _database.GetCollection<Recenzija>("Recenzije");

        public async Task<bool> TestKonekcijaAsync()
        {
            try
            {
                await _client.ListDatabaseNamesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
