using Newtonsoft.Json;
using RestSharp;
using System.Security.Principal;

namespace Innamoramelo.Models
{
    public class UserAPI
    {
        private string UrlAPI { get; set; }
        public UserAPI(IConfiguration config)
        {
            UrlAPI = config["urlAPI"] + "/User/";
        }

        internal async Task<UserDTO?> GetUser(string token)
        {
            try
            {
                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);

                var request = new RestRequest("GetUser", Method.Get);
                request.AddHeader("Authorization", "Bearer " + token);

                RestResponse response = await client.ExecuteAsync(request);
                
                var userDTO = JsonConvert.DeserializeObject<UserDTO>(response.Content);
                return userDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        internal async Task<UserDTO?> GetUserById(string id, string token)
        {
            try
            {
                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);

                var request = new RestRequest("GetUserById?id=" + id, Method.Get);
                request.AddHeader("Authorization", "Bearer " + token);

                RestResponse response = await client.ExecuteAsync(request);

                var userDTO = JsonConvert.DeserializeObject<UserDTO>(response.Content);
                return userDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        internal async Task<UserDTO?> InsertUser(UserCreateViewModel userModel, string token)
        {
            try
            {
                string json = JsonConvert.SerializeObject(userModel);

                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);

                var request = new RestRequest("InsertUser", Method.Post);
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddHeader("Content-Type", "application/json");

                var body = json;
                request.AddStringBody(body, DataFormat.Json);

                RestResponse response = await client.ExecuteAsync(request);
                var userDTO = JsonConvert.DeserializeObject<UserDTO>(response.Content);

                return userDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        internal async Task<UserDTO?> UpdateUser(UserUpdateViewModel userModel, string token)
        {
            try
            {
                string json = JsonConvert.SerializeObject(userModel);

                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);

                var request = new RestRequest("UpdateUser", Method.Patch);
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddHeader("Content-Type", "application/json");

                var body = json;
                request.AddStringBody(body, DataFormat.Json);

                RestResponse response = await client.ExecuteAsync(request);
                var userDTO = JsonConvert.DeserializeObject<UserDTO>(response.Content);

                return userDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        internal async Task<bool?> DeleteUser(string token)
        {
            try
            {
                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);

                var request = new RestRequest("DeleteUser", Method.Delete);
                request.AddHeader("Content-Type", "application/json");

                RestResponse response = await client.ExecuteAsync(request);
                
                if(response.Content == "true")
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
