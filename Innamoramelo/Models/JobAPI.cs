using Newtonsoft.Json;
using RestSharp;

namespace Innamoramelo.Models
{
    public class JobAPI
    {
        private string UrlAPI { get; set; }
        public JobAPI(IConfiguration config)
        {
            UrlAPI = config["urlAPI"] + "/Job/";
        }

        internal async Task<List<JobDTO>?> GetJob(string filter, string token)
        {
            try
            {
                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);

                var request = new RestRequest("GetJob?filter=" + filter, Method.Get);
                request.AddHeader("Authorization", "Bearer " + token);

                RestResponse response = await client.ExecuteAsync(request);

                var jobsDTO = JsonConvert.DeserializeObject<List<JobDTO>?>(response.Content);
                return jobsDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}
