using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using AutoMapper;

namespace InnamorameloAPI.Models
{
    public class SecretCodeAPI
    {
        [BsonIgnoreIfDefault]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        [BsonIgnoreIfDefault]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId IdUser { get; set; }
        public string? Code { get; set; }
        public DateTime? Created { get; set; }

        public SecretCodeAPI() { }
        public SecretCodeAPI(string? code, DateTime? created)
        {
            Code = code;
            Created = created;
        }

        static private MongoAPI mongo = new MongoAPI();

        internal SecretCodeAPI CreateSecretCode()
        {
            Random rnd = new Random();
            int num = rnd.Next(10000, 99999);

            var secretCode = new SecretCodeAPI(num.ToString(), DateTime.Now.AddMinutes(5));

            return secretCode;
        }

        internal SecretCodeDTO GetSecretCode(ObjectId idUser, bool reload = true)
        {
            //BUG: quando salvo la data di creazione su atlas mi cambia l'orario come se ci fosse un fuso orario diverso 
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<SecretCodeAPI> secretCodes = innamoramelo.GetCollection<SecretCodeAPI>("SecretCodes");

                var filter = Builders<SecretCodeAPI>.Filter.Eq(x => x.IdUser, idUser);

                var find = secretCodes.Find(filter).FirstOrDefault();
                if (find != null)
                {
                    find.Created = find.Created.Value.AddHours(1); //Brutta risoluzione del bug sopra citato
                    if (reload && DateTime.Now > find.Created)
                    {
                        var newSecretCode = CreateSecretCode();
                        newSecretCode.IdUser = idUser;
                        InsertSecretCode(newSecretCode);

                        DeleteSecretCode(find.Id);

                        find = newSecretCode;
                    }

                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<SecretCodeAPI, SecretCodeDTO>();
                    });

                    IMapper mapper = config.CreateMapper();

                    var secretCode = mapper.Map<SecretCodeDTO>(find);

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

        internal bool InsertSecretCode(SecretCodeAPI secretCode)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<SecretCodeAPI> secretCodes = innamoramelo.GetCollection<SecretCodeAPI>("SecretCodes");

                secretCodes.InsertOne(secretCode);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        internal bool DeleteSecretCode(ObjectId id)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<SecretCodeAPI> secretCodes = innamoramelo.GetCollection<SecretCodeAPI>("SecretCodes");

                var filter = Builders<SecretCodeAPI>.Filter.Eq(x => x.Id, id);
                var delete = secretCodes.DeleteOne(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
    }

    public class SecretCodeDTO 
    {
        [BsonIgnoreIfDefault]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        [BsonIgnoreIfDefault]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId IdUser { get; set; }
        public string? Code { get; set; }
        public DateTime? Created { get; set; }

        public SecretCodeDTO() { }
        public SecretCodeDTO(string? code, DateTime? created)
        {
            Code = code;
            Created = created;
        }
    }
}
