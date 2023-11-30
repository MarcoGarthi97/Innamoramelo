using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace InnamorameloAPI.Models
{
    public class PhotoAPI
    {
        private readonly string path = File.ReadAllText(@"C:\Users\marco\source\repos\_MyCredentials\Innamoramelo\DirectoryPhoto");
        private static MongoAPI mongo = new MongoAPI();

        internal List<PhotoDTO>? GetPhotosByUserId(string userId)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<PhotoMongoDB> photos = innamoramelo.GetCollection<PhotoMongoDB>("Photos");

                FilterDefinition<PhotoMongoDB> filter = Builders<PhotoMongoDB>.Filter.Eq(x => x.UserId, new ObjectId(userId));
                var find = photos.Aggregate().Match(filter).ToList();

                if (find != null)
                {
                    var photoList = new List<PhotoDTO>();

                    foreach (var photo in find)
                    {
                        var photoDTO = new PhotoDTO();
                        Validator.CopyProperties(find, photoDTO);

                        photoDTO.Bytes = GetPhotoByDirectory(photoDTO.UserId, photoDTO.Id);
                        if(photoDTO.Bytes != null)
                            photoList.Add(photoDTO);
                    }

                    return photoList;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        private Byte[]? GetPhotoByDirectory(string userId, string id)
        {
            try
            {
                var files = Directory.GetFiles(path + @"\" + userId).ToList();
                var file = files.FirstOrDefault(x => x.Contains(id));

                if (file != null)
                {
                    var bytes = File.ReadAllBytes(file);
                    return bytes;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }

    public class PhotoMongoDB : Photo
    {
        [BsonIgnoreIfDefault]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        [BsonIgnoreIfDefault]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId UserId { get; set; }
    }

    public class PhotoDTO : Photo
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public byte[]? Bytes { get; set; }

        public PhotoDTO() { }
        public PhotoDTO(int? position, string? path, byte[]? bytes, int? action)
        {
            Position = position;
            Path = path;
            Bytes = bytes;
            Action = action;
        }
    }

    public class Photo
    {
        public int? Position { get; set; }
        public string? Path { get; set; }
        public int? Action { get; set; }
    }
}
