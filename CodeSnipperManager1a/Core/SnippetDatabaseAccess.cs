using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using CodeSnipperManager1a.MVVM.Model;

using MongoDB.Driver;
using MongoDB.Bson;

namespace CodeSnipperManager1a.Core
{
    public class SnippetDatabaseAccess
    {
        private const string ConnectionUri = "mongodb+srv://admin:uR7b2CXPKKeJR0Sf@code-snippets.ng5weyc.mongodb.net/?retryWrites=true&w=majority";

        private MongoClientSettings settings = MongoClientSettings.FromConnectionString(ConnectionUri);

        private MongoClient client;

        private IMongoDatabase database;

        private string collectionName = "Details";

        public SnippetDatabaseAccess()
        {
            // Set the ServerApi field of the settings object to Stable API version 1
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            // Create a new client and connect to the server
            client = new MongoClient(settings);

            database = client.GetDatabase("Snippets");

            // Send a ping to confirm a successful connection
            try
            {
                var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Debug.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
            }
            catch (Exception ex)
            { 
                Debug.WriteLine(ex);  
            }
        }

        private IMongoCollection<T> GetDBCollection<T>(string collection) 
        {
            return database.GetCollection<T>(collection);
        }

        public async Task<List<Snippet>> GetSnippets() 
        {
            IMongoCollection<Snippet> snippets = GetDBCollection<Snippet>(collectionName);
            IAsyncCursor<Snippet> results = await snippets.FindAsync(_ => true );

            return results.ToList();
        }  
        
        public async Task<List<Snippet>> GetSnippet(string SnippetTitle) 
        {
            IMongoCollection<Snippet> snippets = GetDBCollection<Snippet>(collectionName);
            IAsyncCursor<Snippet> results = await snippets.FindAsync(snippet => snippet.Title == SnippetTitle);

            return results.ToList();
        }

        public Task CreateStore(Snippet snippet) 
        {
            IMongoCollection<Snippet> snippets = GetDBCollection<Snippet>(collectionName);

            return snippets.InsertOneAsync(snippet);
        }

        public Task UpdateStore(Snippet snippet) 
        {
            IMongoCollection<Snippet> snippets = GetDBCollection<Snippet>(collectionName);
            FilterDefinition<Snippet> filter = Builders<Snippet>.Filter.Eq(s => s.Id, snippet.Id);

            return snippets.ReplaceOneAsync(filter, snippet, new ReplaceOptions {IsUpsert = true});
        }

        /*
         TODO: Move to a junk database then delete it over time
         */
        public Task DeleteStore(Snippet snippet) 
        {
            IMongoCollection<Snippet> snippets = GetDBCollection<Snippet>(collectionName);
            FilterDefinition<Snippet> filter = Builders<Snippet>.Filter.Eq(s => s.Id, snippet.Id);

            return snippets.DeleteOneAsync(filter);
        }
    }
}
