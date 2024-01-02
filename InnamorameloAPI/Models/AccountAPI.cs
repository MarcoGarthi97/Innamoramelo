using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace InnamorameloAPI.Models
{
    public class AccountAPI
    {
        private static IConfiguration Config;

        static private MongoAPI mongo;

        public AccountAPI(IConfiguration config)
        {
            Config = config;
            mongo = new MongoAPI(Config);
        }

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
}
