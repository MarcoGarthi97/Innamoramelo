using MongoDB.Bson;

namespace InnamorameloAPI.Models
{
    public class MatchMongoDB
    {
        public ObjectId Id { get; set; }
        public List<ObjectId>? UsersId { get; set; }
    }
}
