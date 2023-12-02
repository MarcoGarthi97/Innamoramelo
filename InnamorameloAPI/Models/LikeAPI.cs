using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Newtonsoft.Json;
using MongoDB.Driver;

namespace InnamorameloAPI.Models
{
    public class LikeAPI
    {
        static private MongoAPI mongo = new MongoAPI();

        internal LikeDTO? GetLike(string id)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<LikeMongoDB> likes = innamoramelo.GetCollection<LikeMongoDB>("Likes");

                var filter = Builders<LikeMongoDB>.Filter.Eq(x => x.Id, new ObjectId(id));
                var find = likes.Find(filter).FirstOrDefault();

                var like = new LikeDTO();
                Validator.CopyProperties(find, like);

                return like;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        internal List<LikeDTO>? GetAllLike(string userId)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<LikeMongoDB> likes = innamoramelo.GetCollection<LikeMongoDB>("Likes");

                var filter = Builders<LikeMongoDB>.Filter.Eq(x => x.UserId, new ObjectId(userId));
                var find = likes.Aggregate().Match(filter).ToList();

                var likeList = new List<LikeDTO>();

                foreach (var likeFind in find)
                {
                    var like = new LikeDTO();
                    Validator.CopyProperties(likeFind, like);

                    likeList.Add(like);
                }

                return likeList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new List<LikeDTO>();
        }

        internal bool IsMatched(string userId, string userIdLiked)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<LikeMongoDB> likes = innamoramelo.GetCollection<LikeMongoDB>("Likes");

                var filter = Builders<LikeMongoDB>.Filter.Eq(x => x.UserId, new ObjectId(userId));
                filter &= Builders<LikeMongoDB>.Filter.Eq(x => x.UserIdLiked, new ObjectId(userIdLiked));
                var find = likes.Find(filter).FirstOrDefault();

                if (find.IsLiked.Value)
                {
                    filter = Builders<LikeMongoDB>.Filter.Eq(x => x.UserId, new ObjectId(userIdLiked));
                    filter &= Builders<LikeMongoDB>.Filter.Eq(x => x.UserIdLiked, new ObjectId(userId));
                    find = likes.Find(filter).FirstOrDefault();

                    if (find.IsLiked.Value)
                        return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        internal bool InsertLike(LikeDTO likeDTO)
        {
            try
            {
                var like = new LikeMongoDB();
                Validator.CopyProperties(likeDTO, like);

                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<LikeMongoDB> likes = innamoramelo.GetCollection<LikeMongoDB>("Likes");

                likes.InsertOne(like);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        internal LikeDTO? UpdateLike(LikeUpdateModel like)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<LikeMongoDB> likes = innamoramelo.GetCollection<LikeMongoDB>("Likes");

                var filter = Builders<LikeMongoDB>.Filter.Eq(x => x.Id, new ObjectId(like.Id));

                var updateDefinition = new List<UpdateDefinition<LikeMongoDB>>();

                if (like.IsLiked != null)
                    updateDefinition.Add(Builders<LikeMongoDB>.Update.Set("IsLiked", like.IsLiked));

                var updateLike = Builders<LikeMongoDB>.Update.Combine(updateDefinition);
                var update = likes.UpdateOne(filter, updateLike);

                var likeUpdated = GetLike(like.Id);
                return likeUpdated;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal bool DeleteLike(string id)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<LikeMongoDB> likes = innamoramelo.GetCollection<LikeMongoDB>("Likes");

                var filter = Builders<LikeMongoDB>.Filter.Eq(x => x.Id, new ObjectId(id));
                likes.DeleteOne(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        internal bool DeleteLikesByUserId(string userId)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<LikeMongoDB> likes = innamoramelo.GetCollection<LikeMongoDB>("Likes");

                var filter = Builders<LikeMongoDB>.Filter.Eq(x => x.UserId, new ObjectId(userId));
                likes.DeleteMany(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }

    public class LikeMongoDB : Like
    {
        [BsonIgnoreIfDefault]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        [BsonIgnoreIfDefault]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId UserId { get; set; }
        [BsonIgnoreIfDefault]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId UserIdLiked { get; set; }
    }

    public class LikeDTO : Like
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? UserIdLiked { get; set; }
    }

    public class LikeInsertModel : Like
    {
        public string? UserId { get; set; }
        public string? UserIdLiked { get; set; }
    }

    public class LikeUpdateModel
    {
        public string? Id { get; set; }
        public bool? IsLiked { get; set; }
    }

    public class LikeDeleteModel
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
    }

    public class Like
    {
        public DateTime? Created { get; set; }
        public bool? IsLiked { get; set; }
    }
}
