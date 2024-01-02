using MongoDB.Bson;
using MongoDB.Driver;
using Org.BouncyCastle.Utilities;

namespace InnamorameloAPI.Models
{
    public class JobAPI
    {
        private static IConfiguration Config;

        static private MongoAPI mongo;

        public JobAPI(IConfiguration config)
        {
            Config = config;
            mongo = new MongoAPI(Config);
        }

        internal List<JobDTO>? GetJob(string _filter)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase("HelpDB");
                IMongoCollection<JobMongoDB> jobs = innamoramelo.GetCollection<JobMongoDB>("Jobs");

                var filter = Builders<JobMongoDB>.Filter.Regex(x => x.Name, new BsonRegularExpression(_filter.ToLower(), "i"));
                var find = jobs.Aggregate().Match(filter).ToList();

                var listJobsDTO = new List<JobDTO>();

                foreach (var job in find)
                {
                    var jobDTO = new JobDTO();
                    Validator.CopyProperties(job, jobDTO);

                    listJobsDTO.Add(jobDTO);
                }                

                return listJobsDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}
