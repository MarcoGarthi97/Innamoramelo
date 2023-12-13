using Innamoramelo.Controllers;
using Newtonsoft.Json;
using RestSharp;

namespace Innamoramelo.Models
{
    public class AuthenticationAPI
    {
        private string UrlAPI {  get; set; }
        public AuthenticationAPI(IConfiguration config)
        {
            UrlAPI = config["urlAPI"] + "/Authentication/";
        }

        internal async Task<TokenDTO?> GetBearerAsync(AuthenticationDTO authenticationDTO)
        {
            try
            {
                string json = JsonConvert.SerializeObject(authenticationDTO);

                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);

                var request = new RestRequest("GetAuthentication", Method.Post);
                request.AddHeader("Content-Type", "application/json");

                var body = json;
                request.AddStringBody(body, DataFormat.Json);

                RestResponse response = await client.ExecuteAsync(request);
                var tokenDTO = JsonConvert.DeserializeObject<TokenDTO>(response.Content);

                return tokenDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        internal async Task<bool?> ValidationBearerAsync(TokenDTO tokenDTO)
        {
            try
            {
                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);

                var request = new RestRequest("CheckAuthentication", Method.Get);
                request.AddHeader("Authorization", "Bearer " + tokenDTO.Bearer);

                RestResponse response = await client.ExecuteAsync(request);

                if(response.Content == "true")
                    return true;
            }
            catch (Exception ex)
            {

            }

            return false;
        }
    }
}
