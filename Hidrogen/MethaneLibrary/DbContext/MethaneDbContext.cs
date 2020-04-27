using MethaneLibrary.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MethaneLibrary.DbContext {
    
    public class MethaneDbContext {

        private readonly IMongoDatabase _database;

        public MethaneDbContext(IOptions<ServerOptions> options) {
            var connection = new MongoClient(options.Value.Connection);
            
            if (connection == null) throw new MongoClientException("Failed to establish connection to Mongo Atlas.");

            _database = connection.GetDatabase(options.Value.Database);
        }

        public IMongoCollection<RuntimeLog> RuntimeLog => _database.GetCollection<RuntimeLog>(nameof(RuntimeLog));
        
        public IMongoCollection<AccountLog> AccountLog => _database.GetCollection<AccountLog>(nameof(AccountLog));
    }
}