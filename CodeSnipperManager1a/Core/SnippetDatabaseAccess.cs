using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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



    }
}
