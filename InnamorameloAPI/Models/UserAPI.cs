using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using AutoMapper;
using InnamorameloAPI.Models;

namespace InnamorameloAPI.Models
{
    public class UserAPI
    {
        static private MongoAPI mongo = new MongoAPI();

        internal UserDTO? GetUserByEmail(string email)
        {
            try
            {
                var filter = Builders<UserMongoDB>.Filter.Eq(x => x.Email, email);
                var user = GetUser(filter);

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        internal UserDTO? GetUserById(string id)
        {
            try
            {
                var filter = Builders<UserMongoDB>.Filter.Eq(x => x.Id, new ObjectId(id));
                var user = GetUser(filter);

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        private UserDTO? GetUser(FilterDefinition<UserMongoDB> filter)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<UserMongoDB> users = innamoramelo.GetCollection<UserMongoDB>("Users");

                var projection = Builders<UserMongoDB>.Projection
                        .Include(x => x.Id)
                        .Include(x => x.Email)
                        .Include(x => x.Name)
                        .Include(x => x.Phone)
                        .Include(x => x.Birthday)
                        .Include(x => x.IsActive)
                        .Include(x => x.CreateProfile);

                var find = users.Find(filter).Project<UserMongoDB>(projection).FirstOrDefault();

                var user = new UserDTO();
                Validator.CopyProperties(find, user);

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        internal bool CheckUser(AuthenticationDTO user, bool onlyUser)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<UserMongoDB> users = innamoramelo.GetCollection<UserMongoDB>("Users");

                var filter = Builders<UserMongoDB>.Filter.Eq(x => x.Email, user.Email);
                if(!onlyUser)
                    filter &= Builders<UserMongoDB>.Filter.Eq(x => x.Password, user.Password);
                
                var projection = Builders<UserMongoDB>.Projection.Include(x => x.Email);

                var find = users.Find(filter).Project<UserMongoDB>(projection).FirstOrDefault();
                if (find != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }      

        internal UserDTO? InsertUser(UserCreateViewModel user)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<UserCreateViewModel> users = innamoramelo.GetCollection<UserCreateViewModel>("Users");

                users.InsertOne(user);

                var userInserted = GetUserByEmail(user.Email);
                return userInserted;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal UserDTO? UpdateUser(UserUpdateViewModel user)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<UserMongoDB> users = innamoramelo.GetCollection<UserMongoDB>("Users");

                var filter = Builders<UserMongoDB>.Filter.Eq(x => x.Id, new ObjectId(user.Id));

                var updateDefinition = new List<UpdateDefinition<UserMongoDB>>();

                if (!string.IsNullOrEmpty(user.Name))
                    updateDefinition.Add(Builders<UserMongoDB>.Update.Set("Name", user.Name));

                if (!string.IsNullOrEmpty(user.Email))
                    updateDefinition.Add(Builders<UserMongoDB>.Update.Set("Email", user.Email));

                if (!string.IsNullOrEmpty(user.Phone))
                    updateDefinition.Add(Builders<UserMongoDB>.Update.Set("Phone", user.Phone));

                if (user.Birthday != null)
                    updateDefinition.Add(Builders<UserMongoDB>.Update.Set("Birthday", user.Birthday));

                var updateUser = Builders<UserMongoDB>.Update.Combine(updateDefinition);
                var update = users.UpdateOne(filter, updateUser);

                var userUpdated = GetUserById(user.Id);
                return userUpdated;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal bool DeleteUser(string id)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<UserMongoDB> users = innamoramelo.GetCollection<UserMongoDB>("Users");

                var filter = Builders<UserMongoDB>.Filter.Eq(x => x.Id, new ObjectId(id));

                var delete = users.DeleteOne(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
    }   
}
public class UserMongoDB : User
{
    [BsonIgnoreIfDefault]
    [JsonConverter(typeof(ObjectIdConverter))]
    public ObjectId Id { get; set; }
    public string Password { get; set; }
}

public class UserDTO : User
{
    public string Id { get; set; }
}

public class User
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime? Birthday { get; set; }
    public bool? IsActive { get; set; }
    public bool? CreateProfile { get; set; }
}

public class UserCreateViewModel
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Phone { get; set; }
    public DateTime? Birthday { get; set; }
}

public class UserUpdateViewModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? Birthday { get; set; }
    }

