using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
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

    //public class ObjectIdConverter : JsonConverter<ObjectId>
    //{
    //    public override ObjectId ReadJson(JsonReader reader, Type objectType, ObjectId existingValue, bool hasExistingValue, JsonSerializer serializer)
    //    {
    //        var token = JToken.Load(reader);
    //        if (token.Type == JTokenType.String)
    //        {
    //            return ObjectId.Parse(token.Value<string>());
    //        }
    //        return ObjectId.Empty;
    //    }

    //    public override void WriteJson(JsonWriter writer, ObjectId value, JsonSerializer serializer)
    //    {
    //        writer.WriteValue(value.ToString());
    //    }
    //}

    class ObjectIdConverter : JsonConverter
    {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ObjectId).IsAssignableFrom(objectType);
            //return true;
        }


    }
}
