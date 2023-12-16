using Newtonsoft.Json;
using RestSharp;

namespace Innamoramelo.Models
{
    public class CityAPI
    {
        private string UrlAPI { get; set; }
        public CityAPI(IConfiguration config)
        {
            UrlAPI = config["urlAPI"] + "/City/";
        }

        internal async Task<List<GeoDTO>?> GetCity(string filter, string token)
        {
            try
            {
                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);

                var request = new RestRequest("GetPlace?filter=" + filter, Method.Get);
                request.AddHeader("Authorization", "Bearer " + token);

                RestResponse response = await client.ExecuteAsync(request);

                var citiesDTO = JsonConvert.DeserializeObject<List<GeoDTO>?>(response.Content);
                citiesDTO = citiesDTO.OrderBy(x => x.Name).ToList();

                return citiesDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}
