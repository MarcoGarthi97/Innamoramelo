using Newtonsoft.Json;
using RestSharp;

namespace Innamoramelo.Models
{
    public class MatchAPI
    {
        private string UrlAPI { get; set; }
        public MatchAPI(IConfiguration config)
        {
            UrlAPI = config["urlAPI"] + "/Match/";
        }

        internal async Task<List<MatchDTO>?> GetMatches(string token)
        {
            try
            {
                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);

                var request = new RestRequest("GetAllMatches", Method.Get);
                request.AddHeader("Authorization", "Bearer " + token);

                RestResponse response = await client.ExecuteAsync(request);

                var matchesDTO = JsonConvert.DeserializeObject<List<MatchDTO>>(response.Content);
                return matchesDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}
