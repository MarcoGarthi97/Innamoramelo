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

        internal async Task<User?> GetUser(User user, bool onlyUser = false)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<User> users = innamoramelo.GetCollection<User>("Users");

                var filter = Builders<User>.Filter.Eq(x => x.Phone, user.Phone);

                if(!onlyUser)
                    filter &= Builders<User>.Filter.Eq(x => x.Password, user.Password);

                var find = users.Find(filter).FirstOrDefault();
                find.Password = null;

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

                if (user.SecretCode != null)
                    updateDefinition.Add(Builders<User>.Update.Set("SecretCode", user.SecretCode));

                if (user.IsActive != null)
                    updateDefinition.Add(Builders<User>.Update.Set("IsActive", user.IsActive));

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

        internal async Task<List<Match?>> GetAllMatch(ObjectId id, int type = 0)
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

                var find = matches.Aggregate().Match(filter).ToList();

                return find;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal async Task<bool> InsertMatch(Match match)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<Match> matches = innamoramelo.GetCollection<Match>("Match");

                matches.InsertOne(match);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
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

        internal async Task<ChatInfos?> GetChatInfo(ObjectId id)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<ChatInfos> chatInfos = innamoramelo.GetCollection<ChatInfos>("ChatInfos");

                var filter = Builders<ChatInfos>.Filter.Eq(x => x.Id, id);

                var find = chatInfos.Find(filter).FirstOrDefault();

                return find;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal async Task<List<ChatInfos>?> GetAllChatInfo(ObjectId UserId)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<ChatInfos> chatInfos = innamoramelo.GetCollection<ChatInfos>("ChatInfos");

                var filter = Builders<ChatInfos>.Filter.ElemMatch(x => x.ChatInfosUsers, u => u.UserId == UserId);

                var find = chatInfos.Aggregate().Match(filter).ToList();

                return find;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal async Task<bool> InsertChatInfos(ChatInfos chat)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<ChatInfos> chatInfos = innamoramelo.GetCollection<ChatInfos>("ChatInfos");

                chatInfos.InsertOne(chat);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal async Task<bool> UpdateChatInfos(ChatInfos chatInfos)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<ChatInfos> _chatInfos = innamoramelo.GetCollection<ChatInfos>("Matches");

                var filter = Builders<ChatInfos>.Filter.Eq(x => x.Id, chatInfos.Id);

                var updateDefinition = new List<UpdateDefinition<ChatInfos>>();

                if (chatInfos.IsActive)
                    updateDefinition.Add(Builders<ChatInfos>.Update.Set("IsActive", chatInfos.IsActive));

                if (chatInfos.ChatInfosUsers != null)
                    updateDefinition.Add(Builders<ChatInfos>.Update.Set("ChatInfosUsers", chatInfos.ChatInfosUsers));

                var update = Builders<ChatInfos>.Update.Combine(updateDefinition);

                var updateResult = _chatInfos.UpdateOne(filter, update);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal async Task<bool> DeleteChatInfos(ObjectId id)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<ChatInfos> chatInfos = innamoramelo.GetCollection<ChatInfos>("ChatInfos");

                var filter = Builders<ChatInfos>.Filter.Eq(x => x.Id, id);

                var delete = chatInfos.DeleteOne(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal async Task<Chat?> GetChat(ObjectId id)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<Chat> chats = innamoramelo.GetCollection<Chat>("Chats");

                var filter = Builders<Chat>.Filter.Eq(x => x.Id, id);

                var find = chats.Find(filter).FirstOrDefault();

                return find;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal async Task<List<Chat?>> GetAllChat(Chat chat)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<Chat> chats = innamoramelo.GetCollection<Chat>("Chats");

                var filter = Builders<Chat>.Filter.Eq(x => x.SenderId, chat.SenderId);
                filter &= Builders<Chat>.Filter.Eq(x => x.ReceiverId, chat.ReceiverId);

                var find = chats.Aggregate().Match(filter).ToList();

                filter = Builders<Chat>.Filter.Eq(x => x.SenderId, chat.ReceiverId);
                filter &= Builders<Chat>.Filter.Eq(x => x.ReceiverId, chat.SenderId);

                find.AddRange(chats.Aggregate().Match(filter).ToList());

                return find;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal async Task<bool> InsertChat(Chat chat)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<Chat> chats = innamoramelo.GetCollection<Chat>("Chats");

                chats.InsertOne(chat);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal async Task<bool> UpdateChat(Chat chat)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<Chat> chats = innamoramelo.GetCollection<Chat>("Chats");

                var filter = Builders<Chat>.Filter.Eq(x => x.Id, chat.Id);

                var updateDefinition = new List<UpdateDefinition<Chat>>();

                if (chat.SenderId != null)
                    updateDefinition.Add(Builders<Chat>.Update.Set("SenderId", chat.SenderId));

                if (chat.ReceiverId != null)
                    updateDefinition.Add(Builders<Chat>.Update.Set("ReceiverId", chat.ReceiverId));

                if (!string.IsNullOrEmpty(chat.Content))
                    updateDefinition.Add(Builders<Chat>.Update.Set("Content", chat.Content));

                if (chat.Timestamp.HasValue)
                    updateDefinition.Add(Builders<Chat>.Update.Set("Timestamp", chat.Timestamp));

                var update = Builders<Chat>.Update.Combine(updateDefinition);

                var updateResult = chats.UpdateOne(filter, update);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal async Task<bool> DeleteChat(ObjectId id)
        {
            try
            {
                IMongoDatabase innamoramelo = GetDatabase();
                IMongoCollection<Chat> chats = innamoramelo.GetCollection<Chat>("Chats");

                var filter = Builders<Chat>.Filter.Eq(x => x.Id, id);

                var delete = chats.DeleteOne(filter);

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
