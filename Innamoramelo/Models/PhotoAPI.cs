using Newtonsoft.Json;
using RestSharp;

namespace Innamoramelo.Models
{
    public class PhotoAPI
    {
        private string UrlAPI { get; set; }
        public PhotoAPI(IConfiguration config)
        {
            UrlAPI = config["urlAPI"] + "/Photo/";
        }

        internal async Task<PhotoDTO?> GetPhotoById(string id, string token)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }

            return null;
        }

        internal async Task<PhotoDTO?> GetPhotoByUserId(string userId, string token)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }

            return null;
        }

        internal async Task<List<PhotoDTO>?> GetPhotos(string token)
        {
            try
            {
                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);
                var request = new RestRequest("GetPhotos", Method.Get);

                request.AddHeader("Authorization", "Bearer " + token);

                RestResponse response = await client.ExecuteAsync(request);

                var photos = JsonConvert.DeserializeObject<List<PhotoDTO>>(response.Content);
                return photos;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        internal async Task<PhotoDTO?> InsertPhoto(byte[] bytes, string token)
        {
            try
            {
                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);
                var request = new RestRequest("InsertPhoto", Method.Post);

                request.AddHeader("Authorization", "Bearer " + token);
                request.AlwaysMultipartFormData = true;
                request.AddFile("file", bytes, "file.png");

                RestResponse response = await client.ExecuteAsync(request);

                var photoDTO = JsonConvert.DeserializeObject<PhotoDTO>(response.Content);
                return photoDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}
