using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using AutoMapper;

namespace InnamorameloAPI.Models
{
    public class UserAPI
    {
        [BsonIgnoreIfDefault]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }
        public bool? IsActive { get; set; }
        public bool? CreateProfile { get; set; }

        public UserAPI() { }
        public UserAPI(bool? isActive)
        {
            IsActive = isActive;
        }
        public UserAPI(string? email, string? password)
        {
            Email = email;
            Password = password;
        }
        public UserAPI(string? phone, string? password, string? email)
        {
            Phone = phone;
            Password = password;
            Email = email;
        }

        static private MongoAPI mongo = new MongoAPI();

        internal UserDTO? GetUser(string email)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<UserAPI> users = innamoramelo.GetCollection<UserAPI>("Users");

                var filter = Builders<UserAPI>.Filter.Eq(x => x.Email, email);
                var projection = Builders<UserAPI>.Projection
                    .Include(x => x.Id)
                    .Include(x => x.Email)
                    .Include(x => x.Name)
                    .Include(x => x.Phone)
                    .Include(x => x.Birthday)
                    .Include(x => x.IsActive)
                    .Include(x => x.CreateProfile);

                var find = users.Find(filter).Project<UserAPI>(projection).FirstOrDefault();

                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<UserAPI, UserDTO>();
                });

                IMapper mapper = config.CreateMapper();

                var user = mapper.Map<UserDTO>(find);

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal bool CheckUser(LoginCredentials user, bool onlyUser)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<UserAPI> users = innamoramelo.GetCollection<UserAPI>("Users");

                var filter = Builders<UserAPI>.Filter.Eq(x => x.Email, user.Email);
                if(!onlyUser)
                    filter &= Builders<UserAPI>.Filter.Eq(x => x.Password, user.Password);
                
                var projection = Builders<UserAPI>.Projection.Include(x => x.Email);

                var find = users.Find(filter).Project<UserAPI>(projection).FirstOrDefault();
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

        internal bool InsertUser(UserAPI user)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<UserAPI> users = innamoramelo.GetCollection<UserAPI>("Users");

                users.InsertOne(user);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal bool UpdateUser(UserAPI user)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<UserAPI> users = innamoramelo.GetCollection<UserAPI>("Users");

                var filter = Builders<UserAPI>.Filter.Eq(x => x.Id, user.Id);

                var updateDefinition = new List<UpdateDefinition<UserAPI>>();

                if (!string.IsNullOrEmpty(user.Name))
                    updateDefinition.Add(Builders<UserAPI>.Update.Set("Name", user.Name));

                if (!string.IsNullOrEmpty(user.Password))
                    updateDefinition.Add(Builders<UserAPI>.Update.Set("Password", user.Password));

                if (!string.IsNullOrEmpty(user.Email))
                    updateDefinition.Add(Builders<UserAPI>.Update.Set("Email", user.Email));

                //if (user.SecretCode != null)
                //    updateDefinition.Add(Builders<User>.Update.Set("SecretCode", user.SecretCode));

                if (user.IsActive != null)
                    updateDefinition.Add(Builders<UserAPI>.Update.Set("IsActive", user.IsActive));

                if (user.CreateProfile != null)
                    updateDefinition.Add(Builders<UserAPI>.Update.Set("CreateProfile", user.CreateProfile));

                var update = Builders<UserAPI>.Update.Combine(updateDefinition);

                var updateResult = users.UpdateOne(filter, update);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal bool DeleteUser(UserAPI user)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();

                var profileAPI = new ProfileAPI();
                var profile = profileAPI.GetProfile(user.Id, 1);
                profileAPI.DeleteProfile(profile.Id);

                IMongoCollection<UserAPI> users = innamoramelo.GetCollection<UserAPI>("Users");

                var filter = Builders<UserAPI>.Filter.Eq(x => x.Id, user.Id);

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

    public class UserDTO
    {
        public ObjectId Id { get; set; }
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
}
