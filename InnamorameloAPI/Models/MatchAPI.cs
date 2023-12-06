using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace InnamorameloAPI.Models
{
    public class MatchAPI
    {
        static private MongoAPI mongo = new MongoAPI();

        internal bool IsMatched(string userId, string receiverId)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<LikeMongoDB> likes = innamoramelo.GetCollection<LikeMongoDB>("Likes");

                var filter = Builders<LikeMongoDB>.Filter.Eq(x => x.UserId, new ObjectId(userId));
                filter &= Builders<LikeMongoDB>.Filter.Eq(x => x.ReceiverId, new ObjectId(receiverId));
                var find = likes.Find(filter).FirstOrDefault();

                if (find != null && find.IsLiked.Value)
                {
                    filter = Builders<LikeMongoDB>.Filter.Eq(x => x.UserId, new ObjectId(receiverId));
                    filter &= Builders<LikeMongoDB>.Filter.Eq(x => x.ReceiverId, new ObjectId(userId));
                    find = likes.Find(filter).FirstOrDefault();

                    if (find != null && find.IsLiked.Value)
                        return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }
}
