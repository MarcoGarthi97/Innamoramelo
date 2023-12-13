using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace InnamorameloAPI.Models
{
    public class MongoAPI
    {
        private string connectionString = File.ReadAllText(@"C:\Users\marco\source\repos\_MyCredentials\Innamoramelo\Mongo.txt");
        internal IMongoDatabase GetDatabase(string db = "Innamoramelo")
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var mongoClient = new MongoClient(settings);

            return mongoClient.GetDatabase(db);
        }
    }
}
