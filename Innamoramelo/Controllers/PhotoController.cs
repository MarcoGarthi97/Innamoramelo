using Innamoramelo.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Innamoramelo.Controllers
{
    public class PhotoController : AuthenticationController
    {
        public PhotoController(IConfiguration _config) : base(_config)
        {
            Config = _config;
        }

        public ActionResult<List<PhotoDTO>?> GetPhotos()
        {
            try
            {
                Authentication();

                var photoAPI = new PhotoAPI(Config);
                var photos = photoAPI.GetPhotos(Token).Result;

                return photos;
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }

        public ActionResult<List<PhotoDTO>?> InsertPhoto([FromBody] List<PhotoViewModel> files)
        {
            try
            {
                Authentication();

                var photoAPI = new PhotoAPI(Config);
                var photos = photoAPI.GetPhotos(Token).Result;

                files = files.OrderBy(x => x.Position).ToList();

                foreach (var file in files)
                {
                    file.Data = Encoding.UTF8.GetBytes(file.Content.ToString());

                    var photo = photos.FirstOrDefault(x => x.Position == file.Position);

                    bool? res;
                    if (photo != null)
                        res = photoAPI.DeletePhoto(photo.Id, Token).Result;

                    var photoDTO = photoAPI.InsertPhoto(file.Data, file.Name, Token).Result;
                }

                photos = photoAPI.GetPhotos(Token).Result;

                return photos;
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }
    }
}
