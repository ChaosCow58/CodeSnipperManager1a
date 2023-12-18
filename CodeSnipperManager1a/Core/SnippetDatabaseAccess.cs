using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

using MongoDB.Driver;
using MongoDB.Bson;

using CodeSnipperManager1a.MVVM.Model;


namespace CodeSnipperManager1a.Core
{
    public class SnippetDatabaseAccess
    {
        private const string ConnectionUri = "mongodb+srv://admin:uR7b2CXPKKeJR0Sf@code-snippets.ng5weyc.mongodb.net/?retryWrites=true&w=majority";

        private MongoClientSettings settings = MongoClientSettings.FromConnectionString(ConnectionUri);

        private MongoClient client;

        private IMongoDatabase database;

        private const string DetailsCollection = "Details";
        private const string UserCollection = "Users";

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
                Debug.WriteLine("Could not connect to Database");
                Debug.WriteLine(ex);  
            }
        }

        private IMongoCollection<T> GetDBCollection<T>(string collection) 
        {
            return database.GetCollection<T>(collection);
        }

        public async Task<List<Snippet>> GetSnippets() 
        {
            IMongoCollection<Snippet> snippets = GetDBCollection<Snippet>(DetailsCollection);
            IAsyncCursor<Snippet> results = await snippets.FindAsync(_ => true );

            return results.ToList();
        }  
        
        public async Task<List<Snippet>> GetSnippet(string SnippetId) 
        {
            IMongoCollection<Snippet> snippets = GetDBCollection<Snippet>(DetailsCollection);
            IAsyncCursor<Snippet> results = await snippets.FindAsync(snippet => snippet.Id == SnippetId);

            return results.ToList();
        }

        public Task CreateSnippet(Snippet snippet) 
        {
            IMongoCollection<Snippet> snippets = GetDBCollection<Snippet>(DetailsCollection);

            return snippets.InsertOneAsync(snippet);
        }

        public Task UpdateSnippet(Snippet snippet) 
        {
            IMongoCollection<Snippet> snippets = GetDBCollection<Snippet>(DetailsCollection);
            FilterDefinition<Snippet> filter = Builders<Snippet>.Filter.Eq(s => s.Id, snippet.Id);

            return snippets.ReplaceOneAsync(filter, snippet, new ReplaceOptions {IsUpsert = true});
        }

        /*
         TODO: Move to a junk database then delete it over time
         */
        public Task DeleteSnippet(Snippet snippet) 
        {
            IMongoCollection<Snippet> snippets = GetDBCollection<Snippet>(DetailsCollection);
            FilterDefinition<Snippet> filter = Builders<Snippet>.Filter.Eq(s => s.Id, snippet.Id);

            return snippets.DeleteOneAsync(filter);
        }

        // Users
        public async Task<List<User>> GetUsers()
        {
            IMongoCollection<User> users = GetDBCollection<User>(UserCollection);
            IAsyncCursor<User> results = await users.FindAsync(_ => true);

            return results.ToList();
        }

        public async Task<List<User>> GetUser(string UserId)
        {
            IMongoCollection<User> users = GetDBCollection<User>(UserCollection);
            IAsyncCursor<User> results = await users.FindAsync(user => user.Id == UserId);

            return results.ToList();
        }

        public Task CreateUser(User user)
        {
            IMongoCollection<User> users = GetDBCollection<User>(UserCollection);

            return users.InsertOneAsync(user);
        }

        public Task UpdateUser(User user)
        {
            IMongoCollection<User> users = GetDBCollection<User>(UserCollection);
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);

            return users.ReplaceOneAsync(filter, user, new ReplaceOptions { IsUpsert = true });
        }

        /*
         TODO: Move to a junk database then delete it over time
         */
        public Task DeleteUser(User user)
        {
            IMongoCollection<User> users = GetDBCollection<User>(UserCollection);
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);

            return users.DeleteOneAsync(filter);
        }
    }
}
