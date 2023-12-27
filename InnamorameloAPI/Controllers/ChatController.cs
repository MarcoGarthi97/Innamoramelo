using InnamorameloAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnamorameloAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        static private AuthenticationAPI auth = new AuthenticationAPI();
        static private MyBadRequest badRequest = new MyBadRequest();

        [HttpGet("GetChat", Name = "GetChat")]
        public ActionResult<ChatDTO> GetChat(string id)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var chatAPI = new ChatAPI();
                        var chatDTO = chatAPI.GetChatById(id);
                        
                        if(chatDTO != null)
                        {
                            var matchDTO = new MatchDTO();
                            matchDTO.UsersId = new List<string>
                            {
                                userDTO.Id,
                                chatDTO.ReceiverId
                            };

                            var matchAPI = new MatchAPI();

                            if (matchAPI.GetMatchByUsersId(matchDTO) != null)
                                return Ok(chatDTO);
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

        [HttpGet("GetChatConversation", Name = "GetChatConversation")]
        public ActionResult<List<ChatDTO>> GetChatConversation(ChatGetConversationModel chatModel)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var matchDTO = new MatchDTO();
                        matchDTO.UsersId = new List<string>
                        {
                            userDTO.Id,
                            chatModel.ReceiverId
                        };

                        var matchAPI = new MatchAPI();

                        if (matchAPI.GetMatchByUsersId(matchDTO) != null)
                        {
                            if (chatModel.Skip == null)
                                chatModel.Skip = 0;

                            if(chatModel.Limit == null)
                                chatModel.Limit = 30;

                            var chatAPI = new ChatAPI();
                            var chats = chatAPI.GetConversation(userDTO.Id, chatModel);

                            return Ok(chats);
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

        [HttpPost("InsertChat", Name = "InsertChat")]
        public ActionResult<bool> InsertChat(ChatInsertModel chatModel)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var matchDTO = new MatchDTO();
                        matchDTO.UsersId = new List<string>
                        {
                            userDTO.Id,
                            chatModel.ReceiverId
                        };

                        var matchAPI = new MatchAPI();

                        if (matchAPI.GetMatchByUsersId(matchDTO) != null)
                        {
                            var chatDTO = new ChatDTO();
                            Validator.CopyProperties(chatModel, chatDTO);
                            chatDTO.UserId = userDTO.Id;

                            var chatAPI = new ChatAPI();
                            var result = chatAPI.Insertchat(chatDTO);

                            return Ok(result);
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

        [HttpPatch("UpdateChat", Name = "UpdateChat")]
        public ActionResult<ChatDTO> UpdateChat(ChatUpdateModel chatModel)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var matchDTO = new MatchDTO();
                        matchDTO.UsersId = new List<string>
                        {
                            userDTO.Id,
                            chatModel.ReceiverId
                        };

                        var matchAPI = new MatchAPI();

                        if (matchAPI.GetMatchByUsersId(matchDTO) != null)
                        {
                            var chatAPI = new ChatAPI();
                            var chat = chatAPI.UpdateChat(chatModel);

                            return Ok(chat);
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

        [HttpDelete("DeleteChat", Name = "DeleteChat")]
        public ActionResult<bool> DeleteChat(string id)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var chatAPI = new ChatAPI();
                        var chat = chatAPI.GetChatsByUserId(id, userDTO.Id);

                        if(chat != null)
                        {
                            var result = chatAPI.DeleteChat(id);
                            return Ok(result);
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

        [HttpDelete("DeleteChatByUserId", Name = "DeleteChatByUserId")]
        public ActionResult<bool> DeleteChatByUserId()
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var chatAPI = new ChatAPI();
                        var result = chatAPI.DeleteChatByUserId(userDTO.Id);

                        return Ok(result);
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
    }
}
