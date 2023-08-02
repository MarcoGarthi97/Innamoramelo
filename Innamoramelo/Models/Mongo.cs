using MongoDB.Bson;
using MongoDB.Driver;

namespace Innamoramelo.Models
{
    public class Mongo
    {
        private string connectionString = File.ReadAllText(@"C:\Users\marco\source\repos\_MyCredentials\Mongo.txt");
        private IMongoDatabase GetDatabase()
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var mongoClient = new MongoClient(settings);

            return mongoClient.GetDatabase("Innamoramelo");
        }

        internal async Task<User?> GetUser(User user)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<User> users = innamoramelo.GetCollection<User>("Users");

                var filter = Builders<User>.Filter.Eq(x => x.Phone, user.Phone);
                filter &= Builders<User>.Filter.Eq(x => x.Password, user.Password);

                var find = users.Find(filter).FirstOrDefault();

                return find;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal async Task<bool> InsertUser(User user)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
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

        internal async Task<bool> UpdateUser(User user)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<User> users = innamoramelo.GetCollection<User>("Users");

                var filter = Builders<User>.Filter.Eq(x => x.Id, user.Id);

                var updateDefinition = new List<UpdateDefinition<User>>();

                if (!string.IsNullOrEmpty(user.Phone))
                    updateDefinition.Add(Builders<User>.Update.Set("Username", user.Phone));

                if (!string.IsNullOrEmpty(user.Password))
                    updateDefinition.Add(Builders<User>.Update.Set("Password", user.Password));

                if (!string.IsNullOrEmpty(user.Email))
                    updateDefinition.Add(Builders<User>.Update.Set("Email", user.Email));

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

        internal async Task<bool> DeleteUser(User user)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();

                var profile = GetProfile(user.Id, 1).Result;
                DeleteProfile(profile.Id);

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

        internal async Task<Profile?> GetProfile(ObjectId id, int type = 0)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<Profile> profiles = innamoramelo.GetCollection<Profile>("Profiles");

                FilterDefinition<Profile> filter;

                if(type == 0)
                    filter = Builders<Profile>.Filter.Eq(x => x.Id, id);
                else if(type == 1)
                    filter = Builders<Profile>.Filter.Eq(x => x.UserId, id);
                else
                    filter = Builders<Profile>.Filter.Empty;

                var find = profiles.Find(filter).FirstOrDefault();

                return find;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal async Task<bool> InsertProfile(Profile profile)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<Profile> profiles = innamoramelo.GetCollection<Profile>("Profiles");

                profiles.InsertOne(profile);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal async Task<bool> UpdateProfile(Profile profile)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<Profile> profiles = innamoramelo.GetCollection<Profile>("Profiles");

                var filter = Builders<Profile>.Filter.Eq(x => x.Id, profile.Id);

                var updateDefinition = new List<UpdateDefinition<Profile>>();

                if (!string.IsNullOrEmpty(profile.Name))
                    updateDefinition.Add(Builders<Profile>.Update.Set("Name", profile.Name));

                if (!string.IsNullOrEmpty(profile.Gender))
                    updateDefinition.Add(Builders<Profile>.Update.Set("Gender", profile.Gender));

                if (!string.IsNullOrEmpty(profile.SexualOrientation))
                    updateDefinition.Add(Builders<Profile>.Update.Set("SexualOrientation", profile.SexualOrientation));

                if (!string.IsNullOrEmpty(profile.LookingFor))
                    updateDefinition.Add(Builders<Profile>.Update.Set("LookingFor", profile.LookingFor));

                if (!string.IsNullOrEmpty(profile.School))
                    updateDefinition.Add(Builders<Profile>.Update.Set("School", profile.School));

                if (!string.IsNullOrEmpty(profile.Work))
                    updateDefinition.Add(Builders<Profile>.Update.Set("Work", profile.Work));

                if (!string.IsNullOrEmpty(profile.Bio))
                    updateDefinition.Add(Builders<Profile>.Update.Set("Bio", profile.Bio));

                if (profile.Passions != null)
                    updateDefinition.Add(Builders<Profile>.Update.Set("Passions", profile.Passions));

                if (profile.Birthday.HasValue)
                    updateDefinition.Add(Builders<Profile>.Update.Set("Birthday", profile.Birthday));

                if (profile.Location != null)
                    updateDefinition.Add(Builders<Profile>.Update.Set("Location", profile.Location));

                if (profile.Photos != null)
                    updateDefinition.Add(Builders<Profile>.Update.Set("Photos", profile.Photos));

                if (profile.RangeKm.HasValue)
                    updateDefinition.Add(Builders<Profile>.Update.Set("RangeKm", profile.RangeKm));

                var update = Builders<Profile>.Update.Combine(updateDefinition);

                var updateResult = profiles.UpdateOne(filter, update);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal async Task<bool> DeleteProfile(ObjectId id)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<Profile> profiles = innamoramelo.GetCollection<Profile>("Profiles");

                var filter = Builders<Profile>.Filter.Eq(x => x.Id, id);

                var delete = profiles.DeleteOne(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal async Task<Match?> GetMatch(ObjectId id)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<Match> matches = innamoramelo.GetCollection<Match>("Matches");

                var filter = Builders<Match>.Filter.Eq(x => x.Id, id);

                var find = matches.Find(filter).FirstOrDefault();

                return find;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal async Task<Match?> GetAllMatch(ObjectId id, int type = 0)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<Match> matches = innamoramelo.GetCollection<Match>("Matches");

                var filter = Builders<Match>.Filter.ElemMatch(x => x.Matches, u => u.UsernameId == id);

                if(type == 0)
                    filter &= Builders<Match>.Filter.Eq(x => x.IsMatch, true);
                else if(type == 1)
                    filter &= Builders<Match>.Filter.Eq(x => x.IsMatch, false);

                var find = matches.Find(filter).FirstOrDefault();

                return find;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal async Task<bool> UpdateMatch(Match match)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<Match> matches = innamoramelo.GetCollection<Match>("Matches");

                var filter = Builders<Match>.Filter.Eq(x => x.Id, match.Id);

                var updateDefinition = new List<UpdateDefinition<Match>>();

                if (match.Matches != null)
                    updateDefinition.Add(Builders<Match>.Update.Set("Matches", match.Matches));

                if (match.IsMatch.HasValue)
                    updateDefinition.Add(Builders<Match>.Update.Set("IsMatch", match.IsMatch));

                var update = Builders<Match>.Update.Combine(updateDefinition);

                var updateResult = matches.UpdateOne(filter, update);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal async Task<bool> DeleteMatch(ObjectId id)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<Match> matches = innamoramelo.GetCollection<Match>("Matches");

                var filter = Builders<Match>.Filter.Eq(x => x.Id, id);

                var delete = matches.DeleteOne(filter);

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
