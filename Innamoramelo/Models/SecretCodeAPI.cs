using Newtonsoft.Json;
using RestSharp;

namespace Innamoramelo.Models
{
    public class SecretCodeAPI
    {
        private string UrlAPI { get; set; }
        public SecretCodeAPI(IConfiguration config)
        {
            UrlAPI = config["urlAPI"] + "/SecretCode/";
        }

        internal async Task<bool?> GetSecretCode(string token)
        {
            try
            {
                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);

                var request = new RestRequest("GetSecretCode", Method.Get);
                request.AddHeader("Authorization", "Bearer " + token);

                RestResponse response = await client.ExecuteAsync(request);

                if (response.Content == "true")
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        internal async Task<bool?> ValidateUser(string code, string token)
        {
            try
            {
                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);

                var request = new RestRequest("ValidateUser?code=" + code, Method.Post);
                request.AddHeader("Authorization", "Bearer " + token);

                RestResponse response = await client.ExecuteAsync(request);

                if (response.Content == "true")
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}
