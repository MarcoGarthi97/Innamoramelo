using MongoDB.Bson;
using Newtonsoft.Json;

namespace InnamorameloAPI.Models
{
    public class ChatMongoDB : Chat
    {
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId? UserId { get; set; }
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId? ReceiverId { get; set; }
    }
}
