using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace InnamorameloAPI.Models
{
    public class AccountAPI
    {
        [BsonIgnoreIfDefault]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Level { get; set; }

        static private MongoAPI mongo = new MongoAPI();

        internal AccountDTO? GetAccount(string username)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<AccountMongoDB> accounts = innamoramelo.GetCollection<AccountMongoDB>("Accounts");

                var filter = Builders<AccountMongoDB>.Filter.Eq(x => x.Username, username);
                var accountMongo = accounts.Find(filter).FirstOrDefault();

                var account = new AccountDTO();
                Validator.CopyProperties(accountMongo, account);

                return account;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal AccountDTO? GetAccount(string username, string password)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<AccountMongoDB> accounts = innamoramelo.GetCollection<AccountMongoDB>("Accounts");

                var filter = Builders<AccountMongoDB>.Filter.Eq(x => x.Username, username);
                filter &= Builders<AccountMongoDB>.Filter.Eq(x => x.Password, password);

                var accountMongo = accounts.Find(filter).FirstOrDefault();

                var account = new AccountDTO();
                Validator.CopyProperties(accountMongo, account);

                return account;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal AccountDTO? InsertAccount(AccountDTO account)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<AccountMongoDB> accounts = innamoramelo.GetCollection<AccountMongoDB>("Accounts");

                var accountMongo = new AccountMongoDB();
                Validator.CopyProperties(account, accountMongo);

                accounts.InsertOne(accountMongo);

                return account;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal bool DeleteAccount(string username)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<AccountMongoDB> accounts = innamoramelo.GetCollection<AccountMongoDB>("Accounts");

                var filter = Builders<AccountMongoDB>.Filter.Eq(x => x.Username, username);

                accounts.DeleteOne(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
    }

    public class AccountMongoDB : Account
    {
        [BsonIgnoreIfDefault]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
    }

    public class AccountDTO : Account
    {
        public string Id { get; set; }
        public AccountDTO() { }
        public AccountDTO(string? username, string? password, string? level)
        {
            Username = username;
            Password = password;
            Level = level;
        }
    }

    public class Account
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Level { get; set; }
    }
}
