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

        static private MongoAPI mongo = new MongoAPI();

        internal UserDTO? GetUserByEmail(string email)
        {
            try
            {
                var filter = Builders<UserAPI>.Filter.Eq(x => x.Email, email);
                var user = GetUser(filter);

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        internal UserDTO? GetUserById(ObjectId id)
        {
            try
            {
                var filter = Builders<UserAPI>.Filter.Eq(x => x.Id, id);
                var user = GetUser(filter);

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        private UserDTO? GetUser(FilterDefinition<UserAPI> filter)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<UserAPI> users = innamoramelo.GetCollection<UserAPI>("Users");

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
            }

            return null;
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

        internal UserDTO? UpdateUser(ObjectId id, UserUpdateViewModel user)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<UserAPI> users = innamoramelo.GetCollection<UserAPI>("Users");

                var filter = Builders<UserAPI>.Filter.Eq(x => x.Id, id);

                var updateDefinition = new List<UpdateDefinition<UserAPI>>();

                if (!string.IsNullOrEmpty(user.Name))
                    updateDefinition.Add(Builders<UserAPI>.Update.Set("Name", user.Name));

                if (!string.IsNullOrEmpty(user.Email))
                    updateDefinition.Add(Builders<UserAPI>.Update.Set("Email", user.Email));

                if (!string.IsNullOrEmpty(user.Phone))
                    updateDefinition.Add(Builders<UserAPI>.Update.Set("Phone", user.Phone));

                if (user.Birthday != null)
                    updateDefinition.Add(Builders<UserAPI>.Update.Set("Birthday", user.Birthday));

                var updateUser = Builders<UserAPI>.Update.Combine(updateDefinition);
                var update = users.UpdateOne(filter, updateUser);

                var userUpdated = GetUserById(id);
                return userUpdated;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal bool DeleteUser(ObjectId id)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<UserAPI> users = innamoramelo.GetCollection<UserAPI>("Users");

                var filter = Builders<UserAPI>.Filter.Eq(x => x.Id, Id);

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

    public class UserUpdateViewModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? Birthday { get; set; }
    }

