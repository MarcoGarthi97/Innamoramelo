using MongoDB.Bson;
using MongoDB.Driver;

namespace InnamorameloAPI.Models
{
    public class MatchAPI
    {
        static private MongoAPI mongo = new MongoAPI();

        internal MatchDTO? GetMatchByIdUsers(MatchDTO matchDTO)
        {
            try
            {
                var matchMongoDB = new MatchMongoDB();
                Validator.CopyProperties(matchDTO, matchMongoDB);

                var filter = Builders<MatchMongoDB>.Filter.AnyIn(x => x.UsersId, matchMongoDB.UsersId);
                matchDTO = GetMatch(filter);

                return matchDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        internal MatchDTO? GetMatchById(string id)
        {
            try
            {
                var filter = Builders<MatchMongoDB>.Filter.Eq(x => x.Id, new ObjectId(id));
                var matchDTO = GetMatch(filter);

                return matchDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        internal MatchDTO? GetMatch(FilterDefinition<MatchMongoDB> filter)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<MatchMongoDB> matches = innamoramelo.GetCollection<MatchMongoDB>("Matches");

                var find = matches.Find(filter).FirstOrDefault();

                if (find != null)
                {
                    var matchDTO = new MatchDTO();
                    Validator.CopyProperties(find, matchDTO);

                    return matchDTO;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        internal List<MatchDTO> GetAllMatches(string userId)
        {
            var matchesDTO = new List<MatchDTO>();

            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<MatchMongoDB> matches = innamoramelo.GetCollection<MatchMongoDB>("Matches");

                var filter = Builders<MatchMongoDB>.Filter.AnyIn(x => x.UsersId, new List<ObjectId> { new ObjectId(userId) });
                var find = matches.Aggregate().Match(filter).ToList();

                foreach (var match in find)
                {
                    var matchDTO = new MatchDTO();
                    Validator.CopyProperties(match, matchDTO);

                    matchesDTO.Add(matchDTO);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return matchesDTO;
        }

        internal MatchDTO InsertMatch(MatchDTO matchDTO)
        {
            try
            {
                var matchMongoDB = new MatchMongoDB();
                Validator.CopyProperties(matchDTO, matchMongoDB);

                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<MatchMongoDB> matches = innamoramelo.GetCollection<MatchMongoDB>("Matches");

                matches.InsertOne(matchMongoDB);

                matchDTO = GetMatchByIdUsers(matchDTO);

                return matchDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        internal bool DeleteMatch(MatchDTO matchDTO)
        {
            try
            {
                var matchMongoDB = new MatchMongoDB();
                Validator.CopyProperties(matchDTO, matchMongoDB);

                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<MatchMongoDB> matches = innamoramelo.GetCollection<MatchMongoDB>("Matches");

                var filter = Builders<MatchMongoDB>.Filter.AnyIn(x => x.UsersId, matchMongoDB.UsersId);

                matches.DeleteOne(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        internal bool DeleteAllMatchByUser(string userId)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<MatchMongoDB> matches = innamoramelo.GetCollection<MatchMongoDB>("Matches");

                var filter = Builders<MatchMongoDB>.Filter.AnyIn(x => x.UsersId, new List<ObjectId> { new ObjectId(userId) });

                matches.DeleteMany(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }
}
