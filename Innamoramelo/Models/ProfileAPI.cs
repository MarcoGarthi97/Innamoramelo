using Newtonsoft.Json;
using RestSharp;

namespace Innamoramelo.Models
{
    public class ProfileAPI
    {
        private string UrlAPI { get; set; }
        public ProfileAPI(IConfiguration config)
        {
            UrlAPI = config["urlAPI"] + "/Profile/";
        }

        internal async Task<ProfileDTO?> GetProfile(string token)
        {
            try
            {
                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);

                var request = new RestRequest("GetProfile", Method.Get);
                request.AddHeader("Authorization", "Bearer " + token);

                RestResponse response = await client.ExecuteAsync(request);

                var profileDTO = JsonConvert.DeserializeObject<ProfileDTO>(response.Content);
                return profileDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        internal async Task<ProfileDTO?> GetProfileById(string id, string token)
        {
            try
            {
                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);

                var request = new RestRequest("GetProfileById?id=" + id, Method.Get);
                request.AddHeader("Authorization", "Bearer " + token);

                RestResponse response = await client.ExecuteAsync(request);

                var profileDTO = JsonConvert.DeserializeObject<ProfileDTO>(response.Content);
                return profileDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        internal async Task<ProfileDTO?> GetProfileByUserId(string userId, string token)
        {
            try
            {
                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);

                var request = new RestRequest("GetProfileById?UserId=" + userId, Method.Get);
                request.AddHeader("Authorization", "Bearer " + token);

                RestResponse response = await client.ExecuteAsync(request);

                var profileDTO = JsonConvert.DeserializeObject<ProfileDTO>(response.Content);
                return profileDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        internal async Task<ProfileDTO?> InsertProfile(ProfileViewModel profileModel, string token)
        {
            try
            {
                string json = JsonConvert.SerializeObject(profileModel);

                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);

                var request = new RestRequest("InsertProfile", Method.Post);
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddHeader("Content-Type", "application/json");

                var body = json;
                request.AddStringBody(body, DataFormat.Json);

                RestResponse response = await client.ExecuteAsync(request);

                var profileDTO = JsonConvert.DeserializeObject<ProfileDTO>(response.Content);
                return profileDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        internal async Task<ProfileDTO?> UpdateProfile(ProfileViewModel profileModel, string token)
        {
            try
            {
                string json = JsonConvert.SerializeObject(profileModel);

                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);

                var request = new RestRequest("DeleteProfile", Method.Patch);
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddHeader("Content-Type", "application/json");

                var body = json;
                request.AddStringBody(body, DataFormat.Json);

                RestResponse response = await client.ExecuteAsync(request);

                var profileDTO = JsonConvert.DeserializeObject<ProfileDTO>(response.Content);
                return profileDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        internal async Task<bool?> DeleteProfile(string token)
        {
            try
            {
                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);

                var request = new RestRequest("DeleteProfile", Method.Delete);
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
