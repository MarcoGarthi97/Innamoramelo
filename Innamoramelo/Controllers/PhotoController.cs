using Innamoramelo.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;

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

        public ActionResult<List<PhotoDTO>?> InsertPhoto(IList<IFormFile> files)
        {
            try
            {
                var photos = new List<PhotoDTO>();
                var photoAPI = new PhotoAPI(Config);

                Authentication();

                var delete = photoAPI.DeletePhotos(Token).Result;

                foreach (var file in files)
                {
                    using (var ms = new MemoryStream())
                    {
                        Authentication();

                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();

                        var photoDTO = photoAPI.InsertPhoto(fileBytes, file.FileName, Token).Result;

                        photos.Add(photoDTO);
                    }
                }

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
