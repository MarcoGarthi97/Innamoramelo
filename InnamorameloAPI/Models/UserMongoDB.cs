using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace InnamorameloAPI.Models
{
    public class UserMongoDB : User
    {
        [BsonIgnoreIfDefault]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        public string Password { get; set; }
    }
}