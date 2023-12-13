using MongoDB.Bson;
using MongoDB.Driver;
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

        internal SecretCodeDTO? GetSecretCodeByUserId(ObjectId userId)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<SecretCodeMongoDB> secretCodes = innamoramelo.GetCollection<SecretCodeMongoDB>("SecretCodes");

                var filter = Builders<SecretCodeMongoDB>.Filter.Eq(x => x.UserId, userId);

                var find = secretCodes.Find(filter).FirstOrDefault();

                var secredCodeDTO = new SecretCodeDTO();
                Validator.CopyProperties(find, secredCodeDTO);

                return secredCodeDTO;
            }
            catch(Exception ex)
            {

            }

            return null;
        }

        internal SecretCodeDTO? GetSecretCode(string userId, bool reload = true)
        {
            //BUG: quando salvo la data di creazione su atlas mi cambia l'orario come se ci fosse un fuso orario diverso 
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<SecretCodeMongoDB> secretCodes = innamoramelo.GetCollection<SecretCodeMongoDB>("SecretCodes");

                var filter = Builders<SecretCodeMongoDB>.Filter.Eq(x => x.UserId, new ObjectId(userId));

                var find = secretCodes.Find(filter).FirstOrDefault();

                if (find != null)
                {
                    find.Created = find.Created.Value.AddHours(1); //Brutta risoluzione del bug sopra citato

                    if (reload && DateTime.Now > find.Created)
                    {
                        DeleteSecretCodeById(find.Id.ToString());

                        var secretCodeDTO = InsertSecretCode(find.UserId.ToString());
                        return secretCodeDTO;
                    }
                    else
                    {
                        var secretCodeDTO = new SecretCodeDTO();
                        Validator.CopyProperties(find, secretCodeDTO);

                        return secretCodeDTO;
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        internal SecretCodeDTO? InsertSecretCode(string userId)
        {
            try
            {
                var secretCode = CreateSecretCode();
                secretCode.UserId = new ObjectId(userId);

                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<SecretCodeMongoDB> secretCodes = innamoramelo.GetCollection<SecretCodeMongoDB>("SecretCodes");

                secretCodes.InsertOne(secretCode);

                var secretCodeDTO = GetSecretCodeByUserId(secretCode.UserId);
                return secretCodeDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
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
                var filter = Builders<SecretCodeMongoDB>.Filter.Eq(x => x.UserId, new ObjectId(userId));
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
}
