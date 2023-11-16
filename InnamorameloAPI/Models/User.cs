using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace InnamorameloAPI.Models
{
    public class User
    {
        [BsonIgnoreIfDefault]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }
        public SecretCode? SecretCode { get; set; }
        public bool? IsActive { get; set; }
        public bool? CreateProfile { get; set; }

        public User() { }
        public User(bool? isActive)
        {
            IsActive = isActive;
        }
        public User(string? email, string? password)
        {
            Email = email;
            Password = password;
        }
        public User(string? phone, string? password, string? email)
        {
            Phone = phone;
            Password = password;
            Email = email;
        }

        static private Mongo mongo = new Mongo();

        internal User? GetUser(User user)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<User> users = innamoramelo.GetCollection<User>("Users");

                var filter = Builders<User>.Filter.Eq(x => x.Email, user.Email);
                filter &= Builders<User>.Filter.Eq(x => x.Password, user.Password);

                var find = users.Find(filter).FirstOrDefault();

                if (find != null)
                    find.Password = null;

                return find;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal User? GetUser(User user, bool onlyUser = false)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<User> users = innamoramelo.GetCollection<User>("Users");

                var filter = Builders<User>.Filter.Eq(x => x.Email, user.Email);

                if (!onlyUser)
                    filter &= Builders<User>.Filter.Eq(x => x.Password, user.Password);

                var find = users.Find(filter).FirstOrDefault();

                if (find != null)
                    find.Password = null;

                return find;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }        

        internal bool InsertUser(User user)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<User> users = innamoramelo.GetCollection<User>("Users");

                users.InsertOne(user);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal bool UpdateUser(User user)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<User> users = innamoramelo.GetCollection<User>("Users");

                var filter = Builders<User>.Filter.Eq(x => x.Id, user.Id);

                var updateDefinition = new List<UpdateDefinition<User>>();

                if (!string.IsNullOrEmpty(user.Name))
                    updateDefinition.Add(Builders<User>.Update.Set("Name", user.Name));

                if (!string.IsNullOrEmpty(user.Password))
                    updateDefinition.Add(Builders<User>.Update.Set("Password", user.Password));

                if (!string.IsNullOrEmpty(user.Email))
                    updateDefinition.Add(Builders<User>.Update.Set("Email", user.Email));

                if (user.SecretCode != null)
                    updateDefinition.Add(Builders<User>.Update.Set("SecretCode", user.SecretCode));

                if (user.IsActive != null)
                    updateDefinition.Add(Builders<User>.Update.Set("IsActive", user.IsActive));

                if (user.CreateProfile != null)
                    updateDefinition.Add(Builders<User>.Update.Set("CreateProfile", user.CreateProfile));

                var update = Builders<User>.Update.Combine(updateDefinition);

                var updateResult = users.UpdateOne(filter, update);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal bool DeleteUser(User user)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();

                var profileAPI = new Profile();
                var profile = profileAPI.GetProfile(user.Id, 1);
                profileAPI.DeleteProfile(profile.Id);

                IMongoCollection<User> users = innamoramelo.GetCollection<User>("Users");

                var filter = Builders<User>.Filter.Eq(x => x.Id, user.Id);

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
