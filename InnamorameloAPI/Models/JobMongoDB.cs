using MongoDB.Bson;

namespace InnamorameloAPI.Models
{
    public class JobMongoDB : Job
    {
        public ObjectId Id { get; set; }
    }
}
