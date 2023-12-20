using MongoDB.Bson;
using MongoDB.Driver;

namespace InnamorameloAPI.Models
{
    public class PhotoAPI
    {
        private readonly string path = File.ReadAllText(@"C:\Users\marco\source\repos\_MyCredentials\Innamoramelo\DirectoryPhoto.txt");
        private static MongoAPI mongo = new MongoAPI();

        internal PhotoDTO? GetPhotoById(ObjectId id)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<PhotoMongoDB> photos = innamoramelo.GetCollection<PhotoMongoDB>("Photos");

                FilterDefinition<PhotoMongoDB> filter = Builders<PhotoMongoDB>.Filter.Eq(x => x.Id, id);
                var find = photos.Find(filter).FirstOrDefault();

                if (find != null)
                {
                    var photoDTO = new PhotoDTO();
                    Validator.CopyProperties(find, photoDTO);

                    photoDTO.Bytes = GetPhotoByDirectory(photoDTO.UserId, photoDTO.Name);

                    return photoDTO;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

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
                        Validator.CopyProperties(photo, photoDTO);

                        photoDTO.Bytes = GetPhotoByDirectory(photoDTO.UserId, photoDTO.Name);
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

        internal PhotoMongoDB? GetInfoPhotoByName(string name, ObjectId userId)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<PhotoMongoDB> photos = innamoramelo.GetCollection<PhotoMongoDB>("Photos");

                FilterDefinition<PhotoMongoDB> filter = Builders<PhotoMongoDB>.Filter.Eq(x => x.UserId, userId);
                filter &= Builders<PhotoMongoDB>.Filter.Eq(x => x.Name, name);
                var find = photos.Find(filter).FirstOrDefault(); ;

                return find;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        private Byte[]? GetPhotoByDirectory(string userId, string name)
        {
            try
            {
                var dir = path + @"\" + userId;

                var files = Directory.GetFiles(dir).ToList();
                var file = files.FirstOrDefault(x => x == dir + @"\" + name);

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

        internal PhotoDTO? InsertPhoto(PhotoDTO photoDTO)
        {
            try
            {
                var photo = new PhotoMongoDB();
                Validator.CopyProperties(photoDTO, photo);

                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<PhotoMongoDB> photos = innamoramelo.GetCollection<PhotoMongoDB>("Photos");

                photos.InsertOne(photo);

                var photoInserted = GetInfoPhotoByName(photo.Name, photo.UserId);
                if(photoInserted != null)
                {
                    photoDTO.Id = photoInserted.Id.ToString();
                    var insert = InsertPhotosByDirectory(photoDTO);

                    var update = new PhotoViewModel(photoDTO.Id, photoDTO.Id + photoDTO.Extension, photoDTO.Position);
                    var result = UpdatePhoto(update);
                    result.Bytes = photoDTO.Bytes;

                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        private bool InsertPhotosByDirectory(PhotoDTO photoDTO)
        {
            try
            {
                string dir = path + @"\" + photoDTO.UserId;
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                File.WriteAllBytes(dir + @"\" + photoDTO.Id + photoDTO.Extension, photoDTO.Bytes);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        internal PhotoDTO? UpdatePhoto(PhotoViewModel photoModel)
        {
            try
            {
                var photo = new PhotoMongoDB();
                Validator.CopyProperties(photoModel, photo);

                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<PhotoMongoDB> photos = innamoramelo.GetCollection<PhotoMongoDB>("Photos");

                var filter = Builders<PhotoMongoDB>.Filter.Eq(x => x.Id, photo.Id);

                var updateDefinition = new List<UpdateDefinition<PhotoMongoDB>>();

                if (photo.Name != null)
                    updateDefinition.Add(Builders<PhotoMongoDB>.Update.Set("Name", photo.Name));

                if (photo.Position != null)
                    updateDefinition.Add(Builders<PhotoMongoDB>.Update.Set("Position", photo.Position));

                var updateProfile = Builders<PhotoMongoDB>.Update.Combine(updateDefinition);
                var update = photos.UpdateOne(filter, updateProfile);

                var photoUpdated = GetPhotoById(photo.Id);
                return photoUpdated;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        internal bool DeletePhotoById(PhotoDTO photoDTO)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<PhotoMongoDB> photos = innamoramelo.GetCollection<PhotoMongoDB>("Photos");

                var filter = Builders<PhotoMongoDB>.Filter.Eq(x => x.Id, new ObjectId(photoDTO.Id));
                photos.DeleteOne(filter);

                var result = DeletePhotoByDirectory(photoDTO.UserId, photoDTO.Name);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        internal bool DeletePhotosByIdUser(string userId)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<PhotoMongoDB> photos = innamoramelo.GetCollection<PhotoMongoDB>("Photos");

                var filter = Builders<PhotoMongoDB>.Filter.Eq(x => x.UserId, new ObjectId(userId));
                photos.DeleteMany(filter);

                var result = DeletePhotoByDirectory(userId);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        private bool DeletePhotoByDirectory(string userId)
        {
            try
            {
                var dir = path + @"\" + userId;
                Directory.Delete(dir, true);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        private bool DeletePhotoByDirectory(string userId, string name)
        {
            try
            {
                var file = path + @"\" + userId + @"\" + name;
                File.Delete(file);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }
}
