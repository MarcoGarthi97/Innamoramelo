using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Linq;

namespace InnamorameloAPI.Models
{
    public class ChatAPI
    {
        static private MongoAPI mongo = new MongoAPI();

        public ChatDTO? GetChatById(string id)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<ChatMongoDB> chats = innamoramelo.GetCollection<ChatMongoDB>("Chats");

                var filter = Builders<ChatMongoDB>.Filter.Eq(x => x.Id, new ObjectId(id));
                var find = chats.Find(filter).FirstOrDefault();

                var chat = new ChatDTO();
                Validator.CopyProperties(find, chat);

                return chat;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public ChatDTO? GetChatsByUserId(string id, string userId)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<ChatMongoDB> chats = innamoramelo.GetCollection<ChatMongoDB>("Chats");

                var filter = Builders<ChatMongoDB>.Filter.Eq(x => x.UserId, new ObjectId(userId));
                filter &= Builders<ChatMongoDB>.Filter.Eq(x => x.Id, new ObjectId(id));
                var find = chats.Find(filter).FirstOrDefault();

                var chat = new ChatDTO();
                Validator.CopyProperties(find, chat);

                return chat;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public List<ChatDTO>? GetConversation(string id, ChatGetConversationModel chatModel)
        {
            try
            {
                var chatList = GetConversation(id, chatModel.ReceiverId, chatModel.Skip.Value, chatModel.Limit.Value);
                chatList.AddRange(GetConversation(chatModel.ReceiverId, id, chatModel.Skip.Value, chatModel.Limit.Value));

                chatList = chatList.OrderByDescending(x => x.Timestamp).Take(chatModel.Limit.Value).ToList();

                return chatList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public List<ChatDTO>? GetConversation(string id, string receiverId, int skip, int limit)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<ChatMongoDB> chats = innamoramelo.GetCollection<ChatMongoDB>("Chats");

                var filter = Builders<ChatMongoDB>.Filter.Eq(x => x.UserId, new ObjectId(id));
                filter &= Builders<ChatMongoDB>.Filter.Eq(x => x.ReceiverId, new ObjectId(receiverId));
                var find = chats.Find(filter).Skip(skip).Limit(limit).SortByDescending(x => x.Timestamp).ToList();

                var chatList = new List<ChatDTO>();

                foreach (var chatFind in find)
                {
                    var chat = new ChatDTO();
                    Validator.CopyProperties(chatFind, chat);

                    chatList.Add(chat);
                }

                return chatList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public List<ChatDTO>? GetChatsByReceiverId(ChatGetConversationModel chatModel)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<ChatMongoDB> chats = innamoramelo.GetCollection<ChatMongoDB>("Chats");

                var filter = Builders<ChatMongoDB>.Filter.Eq(x => x.ReceiverId, new ObjectId(chatModel.ReceiverId));
                var find = chats.Find(filter).Skip(chatModel.Skip).Limit(chatModel.Limit).SortByDescending(x => x.Timestamp).ToList();

                var chatList = new List<ChatDTO>();

                foreach (var chatFind in find)
                {
                    var chat = new ChatDTO();
                    Validator.CopyProperties(chatFind, chat);

                    chatList.Add(chat);
                }

                return chatList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public bool Insertchat(ChatDTO chatDTO)
        {
            try
            {
                var chat = new ChatMongoDB();
                Validator.CopyProperties(chatDTO, chat);

                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<ChatMongoDB> chats = innamoramelo.GetCollection<ChatMongoDB>("Chats");

                chats.InsertOne(chat);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        internal ChatDTO? UpdateChat(ChatUpdateModel chat)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<ChatMongoDB> likes = innamoramelo.GetCollection<ChatMongoDB>("Chats");

                var filter = Builders<ChatMongoDB>.Filter.Eq(x => x.Id, new ObjectId(chat.Id));

                var updateDefinition = new List<UpdateDefinition<ChatMongoDB>>();

                if (!string.IsNullOrEmpty(chat.Content))
                    updateDefinition.Add(Builders<ChatMongoDB>.Update.Set("Content", chat.Content));

                if(chat.Viewed != null)
                    updateDefinition.Add(Builders<ChatMongoDB>.Update.Set("Viewed", chat.Viewed));

                var updateChat = Builders<ChatMongoDB>.Update.Combine(updateDefinition);
                var update = likes.UpdateOne(filter, updateChat);

                var chatUpdated = GetChatById(chat.Id);
                return chatUpdated;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal bool DeleteChat(string id)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<ChatMongoDB> chats = innamoramelo.GetCollection<ChatMongoDB>("Chats");

                var filter = Builders<ChatMongoDB>.Filter.Eq(x => x.Id, new ObjectId(id));
                chats.DeleteOne(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        internal bool DeleteChatByUserId(string userId)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<ChatMongoDB> chats = innamoramelo.GetCollection<ChatMongoDB>("Chats");

                var filter = Builders<ChatMongoDB>.Filter.Eq(x => x.UserId, new ObjectId(userId));
                chats.DeleteMany(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        internal bool DeleteChatByReceiverId(string receiverId)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<ChatMongoDB> chats = innamoramelo.GetCollection<ChatMongoDB>("Chats");

                var filter = Builders<ChatMongoDB>.Filter.Eq(x => x.ReceiverId, new ObjectId(receiverId));
                chats.DeleteMany(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }

    public class ChatMongoDB : Chat
    {
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId? UserId { get; set; }
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId? ReceiverId { get; set; }
    }

    public class ChatDTO : Chat
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? ReceiverId { get; set; }
    }

    public class ChatGetConversationModel
    {
        public string? ReceiverId { get; set; }
        public int? Skip { get; set; }
        public int? Limit { get; set; }
    }

    public class ChatInsertModel
    {
        public string? ReceiverId { get; set; }
        public string? Content { get; set; }
        public DateTime? Timestamp { get; set; }
    }

    public class ChatUpdateModel
    {
        public string? Id { get; set; }
        public string? ReceiverId { get; set; }
        public string? Content { get; set; }
        public DateTime? Viewed { get; set; }
    }

    public class Chat
    {
        public string? Content { get; set; }
        public DateTime? Timestamp { get; set; }
        public DateTime? Viewed { get; set; }
    }
}
