using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace InnamorameloAPI.Models
{
    public class MatchAPI
    {
        static private MongoAPI mongo = new MongoAPI();

        internal MatchDTO? GetMatchByUsersId(MatchDTO matchDTO)
        {
            try
            {
                //var matchMongoDB = new MatchMongoDB();
                //Validator.CopyProperties(matchDTO, matchMongoDB);

                var matchMongoDB = CopyForMongo(matchDTO);

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

        private MatchDTO? GetMatch(FilterDefinition<MatchMongoDB> filter)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<MatchMongoDB> matches = innamoramelo.GetCollection<MatchMongoDB>("Matches");

                var find = matches.Find(filter).FirstOrDefault();

                if (find != null)
                {
                    //var matchDTO = new MatchDTO();
                    //Validator.CopyProperties(find, matchDTO);
                    var matchDTO = CopyForDTO(find);

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
                    //var matchDTO = new MatchDTO();
                    //Validator.CopyProperties(match, matchDTO);
                    var matchDTO = CopyForDTO(match);

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
                //var matchMongoDB = new MatchMongoDB();
                //Validator.CopyProperties(matchDTO, matchMongoDB);
                var matchMongoDB = CopyForMongo(matchDTO);

                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<MatchMongoDB> matches = innamoramelo.GetCollection<MatchMongoDB>("Matches");

                matches.InsertOne(matchMongoDB);

                matchDTO = GetMatchByUsersId(matchDTO);

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
                //var matchMongoDB = new MatchMongoDB();
                //Validator.CopyProperties(matchDTO, matchMongoDB);
                var matchMongoDB = CopyForMongo(matchDTO);

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

        private MatchMongoDB CopyForMongo(MatchDTO matchDTO)
        {
            var matchMongoDB = new MatchMongoDB();

            try
            {
                if(matchDTO.Id  != null)
                {
                    matchMongoDB.Id = new ObjectId(matchDTO.Id);
                }

                if(matchDTO.UsersId != null)
                {
                    matchMongoDB.UsersId = new List<ObjectId>()
                    {
                        new ObjectId(matchDTO.UsersId[0]),
                        new ObjectId(matchDTO.UsersId[1])
                    };
                }
            }
            catch(Exception ex)
            {

            }

            return matchMongoDB;
        }

        private MatchDTO CopyForDTO(MatchMongoDB matchMongoDB)
        {
            var matchDTO = new MatchDTO();

            try
            {
                matchDTO.Id = matchMongoDB.Id.ToString();

                if(matchMongoDB.UsersId != null)
                {
                    matchDTO.UsersId = new List<string>()
                    {
                        matchMongoDB.UsersId[0].ToString(),
                        matchMongoDB.UsersId[1].ToString()
                    };
                }
            }
            catch (Exception ex)
            {

            }

            return matchDTO;
        }
    }
}
