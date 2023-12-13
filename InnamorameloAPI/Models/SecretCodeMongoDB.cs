using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace InnamorameloAPI.Models
{
    public class SecretCodeMongoDB : SecretCode
    {

        [BsonIgnoreIfDefault]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        [BsonIgnoreIfDefault]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId UserId { get; set; }
        public SecretCodeMongoDB() { }
        public SecretCodeMongoDB(string? code, DateTime? created)
        {
            Code = code;
            Created = created;
        }
    }
}
