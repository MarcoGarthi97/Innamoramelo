using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using AutoMapper;

namespace InnamorameloAPI.Models
{
    public class SecretCodeAPI
    {
        static private MongoAPI mongo = new MongoAPI();

        internal SecretCodeMongoDB CreateSecretCode()
        {
            Random rnd = new Random();
            int num = rnd.Next(10000, 99999);

            var secretCode = new SecretCodeMongoDB(num.ToString(), DateTime.Now.AddMinutes(5));

            return secretCode;
        }

        internal SecretCodeDTO? GetSecretCode(string userId, bool reload = true)
        {
            //BUG: quando salvo la data di creazione su atlas mi cambia l'orario come se ci fosse un fuso orario diverso 
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<SecretCodeMongoDB> secretCodes = innamoramelo.GetCollection<SecretCodeMongoDB>("SecretCodes");

                var filter = Builders<SecretCodeMongoDB>.Filter.Eq(x => x.IdUser, new ObjectId(userId));

                var find = secretCodes.Find(filter).FirstOrDefault();
                if (find != null)
                {
                    find.Created = find.Created.Value.AddHours(1); //Brutta risoluzione del bug sopra citato
                    if (reload && DateTime.Now > find.Created)
                    {
                        var newSecretCode = CreateSecretCode();
                        newSecretCode.IdUser = new ObjectId(userId);
                        InsertSecretCode(newSecretCode);

                        DeleteSecretCodeById(find.Id.ToString());

                        find = newSecretCode;
                    }

                    var secretCode = new SecretCodeDTO();
                    Validator.CopyProperties(find, secretCode);

                    return secretCode;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal bool InsertSecretCode(SecretCodeMongoDB secretCode)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<SecretCodeMongoDB> secretCodes = innamoramelo.GetCollection<SecretCodeMongoDB>("SecretCodes");

                secretCodes.InsertOne(secretCode);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        internal bool DeleteSecretCodeById(string id)
        {
            try
            {
                var filter = Builders<SecretCodeMongoDB>.Filter.Eq(x => x.Id, new ObjectId(id));
                var delete = DeleteSecretCode(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal bool DeleteSecretCodeByUserId(string userId)
        {
            try
            {
                var filter = Builders<SecretCodeMongoDB>.Filter.Eq(x => x.IdUser, new ObjectId(userId));
                var delete = DeleteSecretCode(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        private bool DeleteSecretCode(FilterDefinition<SecretCodeMongoDB> filter)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<SecretCodeMongoDB> secretCodes = innamoramelo.GetCollection<SecretCodeMongoDB>("SecretCodes");

                var delete = secretCodes.DeleteOne(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal bool ValidateUser(string id)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<SecretCodeMongoDB> users = innamoramelo.GetCollection<SecretCodeMongoDB>("Users");

                var filter = Builders<SecretCodeMongoDB>.Filter.Eq(x => x.Id, new ObjectId(id));

                var updateDefinition = new List<UpdateDefinition<SecretCodeMongoDB>>();
                updateDefinition.Add(Builders<SecretCodeMongoDB>.Update.Set("IsActive", true));

                var updateUser = Builders<SecretCodeMongoDB>.Update.Combine(updateDefinition);
                var update = users.UpdateOne(filter, updateUser);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
    }

    public class SecretCodeMongoDB : SecretCode
    {

        [BsonIgnoreIfDefault]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        [BsonIgnoreIfDefault]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId IdUser { get; set; }
        public SecretCodeMongoDB() { }
        public SecretCodeMongoDB(string? code, DateTime? created)
        {
            Code = code;
            Created = created;
        }
    }

    public class SecretCodeDTO : SecretCode
    {
        public string Id { get; set; }
        public string IdUser { get; set; }

        public SecretCodeDTO() { }
        public SecretCodeDTO(string? code, DateTime? created)
        {
            Code = code;
            Created = created;
        }
    }

    public class SecretCode
    {
        public string? Code { get; set; }
        public DateTime? Created { get; set; }
    }
}
