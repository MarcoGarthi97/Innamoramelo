using MongoDB.Bson;

namespace InnamorameloAPI.Models
{
    public class CityMongoDB : City
    {
        public ObjectId Id { get; set; }
    }
}
