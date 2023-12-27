using InnamorameloAPI.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;

namespace InnamorameloAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LikeController : ControllerBase
    {
        static private AuthenticationAPI auth = new AuthenticationAPI();
        static private MyBadRequest badRequest = new MyBadRequest();

        [HttpGet("GetLike", Name = "GetLike")]
        public ActionResult<LikeDTO> GetLike(string id)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var likeAPI = new LikeAPI();
                        var likes = likeAPI.GetAllLike(userDTO.Id);
                        var like = likes.FirstOrDefault(x => x.Id == id);

                        if(like != null)
                            return Ok(like);
                    }
                    else
                        return badRequest.CreateBadRequest("Unauthorized", "User not authorizated", 404);
                }
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }

        [HttpGet("GetLikebyId", Name = "GetLikebyId")]
        public ActionResult<LikeDTO> GetLikebyId(string id)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    if (auth.CheckLevelUserByToken(authHeader))
                    {
                        var likeAPI = new LikeAPI();
                        var like = likeAPI.GetLike(id);

                        if (like != null)
                            return Ok(like);
                    }
                    else
                        return badRequest.CreateBadRequest("Unauthorized", "User not authorizated", 404);
                }
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }

        [HttpGet("GetAllLikes", Name = "GetAllLikes")]
        public ActionResult<List<LikeDTO>> GetAllLikes()
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var likeAPI = new LikeAPI();
                        var likes = likeAPI.GetAllLike(userDTO.Id);

                        if (likes != null)
                            return Ok(likes);
                    }
                    else
                        return badRequest.CreateBadRequest("Unauthorized", "User not authorizated", 404);
                }
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }

        [HttpPost("InsertLike", Name = "InsertLike")]
        public ActionResult<LikeDTO> InsertLike(LikeInsertModel likeInsert)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    if (auth.CheckLevelUserByToken(authHeader))
                    {
                        var likeDTO = new LikeDTO();
                        Validator.CopyProperties(likeInsert, likeDTO);

                        var likeAPI = new LikeAPI();
                        var likes = likeAPI.GetAllLike(likeDTO.UserId);
                        var like = likes.FirstOrDefault(x => x.ReceiverId == likeDTO.ReceiverId);

                        if (like == null)
                        {
                            var insert = likeAPI.InsertLike(likeDTO);

                            if (insert)
                            {
                                likes = likeAPI.GetAllLike(likeDTO.UserId);
                                like = likes.FirstOrDefault(x => x.ReceiverId == likeDTO.ReceiverId);

                                MatchInsert(likeDTO.UserId, likeDTO.ReceiverId);

                                if (like != null)
                                    return Ok(like);
                            }
                        }
                    }
                    else
                        return badRequest.CreateBadRequest("Unauthorized", "User not authorizated", 404);
                }
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }

        [HttpPatch("UpdateLike", Name = "UpdateLike")]
        public ActionResult<LikeDTO> UpdateLike(LikeUpdateModel likeUpdate)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    if (auth.CheckLevelUserByToken(authHeader))
                    {
                        var likeAPI = new LikeAPI();
                        var likeDTO = likeAPI.UpdateLike(likeUpdate);

                        if (likeUpdate.IsLiked.Value)
                            MatchInsert(likeDTO.UserId, likeDTO.ReceiverId);
                        else
                            MatchDelete(likeDTO.UserId, likeDTO.ReceiverId);

                        if (likeDTO != null)
                            return Ok(likeDTO);
                    }
                    else
                        return badRequest.CreateBadRequest("Unauthorized", "User not authorizated", 404);
                }
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }

        [HttpDelete("DeleteLike", Name = "DeleteLike")]
        public ActionResult<bool> DeleteLike(LikeDeleteModel likeDelete)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    if (auth.CheckLevelUserByToken(authHeader))
                    {
                        var likeAPI = new LikeAPI();
                        var likes = likeAPI.GetAllLike(likeDelete.UserId);
                        var likeDTO = likes.FirstOrDefault(x => x.Id == likeDelete.Id);

                        if (likeDTO != null)
                        {
                            var delete = likeAPI.DeleteLike(likeDelete.Id);

                            MatchInsert(likeDTO.UserId, likeDTO.ReceiverId);

                            if (delete != null)
                                return Ok(delete);
                        }
                    }
                    else
                        return badRequest.CreateBadRequest("Unauthorized", "User not authorizated", 404);
                }
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }

        [HttpDelete("DeleteAllLikes", Name = "DeleteAllLikes")]
        public ActionResult<bool> DeleteAllLikes()
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var likeAPI = new LikeAPI();
                        var delete = likeAPI.DeleteLikesByUserId(userDTO.Id);

                        if (delete != null)
                            return Ok(delete);
                    }
                    else
                        return badRequest.CreateBadRequest("Unauthorized", "User not authorizated", 404);
                }
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }

        private void MatchInsert(string userId, string receiverId)
        {
            try
            {
                var likeAPI = new LikeAPI();
                var likes = likeAPI.GetAllLike(receiverId);
                var like = likes.FirstOrDefault(x => x.ReceiverId == userId);

                if (like.IsLiked.Value)
                {
                    var matchDTO = new MatchDTO();
                    matchDTO.UsersId = new List<string>
                    {
                        userId,
                        receiverId
                    };

                    var matchAPI = new MatchAPI();
                    if(matchAPI.GetMatchByUsersId(matchDTO) == null)
                        matchDTO = matchAPI.InsertMatch(matchDTO);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void MatchDelete(string userId, string receiverId)
        {
            try
            {
                var likeAPI = new LikeAPI();
                var likes = likeAPI.GetAllLike(receiverId);
                var like = likes.FirstOrDefault(x => x.ReceiverId == userId);

                if (like.IsLiked.Value)
                {
                    var matchDTO = new MatchDTO();
                    matchDTO.UsersId = new List<string>
                    {
                        userId,
                        receiverId
                    };

                    var matchAPI = new MatchAPI();
                    var result = matchAPI.DeleteMatch(matchDTO);

                    var chatAPI = new ChatAPI();
                    result = chatAPI.DeleteChatByUserId(userId);
                    result = chatAPI.DeleteChatByReceiverId(receiverId);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
