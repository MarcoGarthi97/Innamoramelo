using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace InnamorameloAPI.Models
{
    public class MongoAPI
    {
        private string connectionString;
        public MongoAPI(IConfiguration config)
        {
            connectionString = File.ReadAllText(config["CredentialsMongoDB"]);
        }

        internal IMongoDatabase GetDatabase(string db = "Innamoramelo")
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var mongoClient = new MongoClient(settings);

            return mongoClient.GetDatabase(db);
        }
    }
}
