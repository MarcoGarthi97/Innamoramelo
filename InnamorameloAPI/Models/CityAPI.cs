using MongoDB.Bson;
using MongoDB.Driver;

namespace InnamorameloAPI.Models
{
    public class CityAPI
    {
        static private MongoAPI mongo = new MongoAPI();

        public List<CityDTO>? GetCity(string _filter)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase("HelpDB");
                IMongoCollection<CityMongoDB> cities = innamoramelo.GetCollection<CityMongoDB>("Comuni");

                var filter = Builders<CityMongoDB>.Filter.Regex(x => x.Name, new BsonRegularExpression(_filter.ToLower(), "i"));
                var find = cities.Aggregate().Match(filter).ToList();

                var listCitiesDTO = new List<CityDTO>();

                foreach (var city in find)
                {
                    var cityDTO = new CityDTO();
                    Validator.CopyProperties(city, cityDTO);

                    listCitiesDTO.Add(cityDTO);
                }

                return listCitiesDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}
