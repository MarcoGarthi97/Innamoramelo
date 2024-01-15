using Innamoramelo.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Innamoramelo.Controllers
{
    public class ChatController : AuthenticationController
    {
        public ChatController(IConfiguration _config) : base(_config)
        {
            Config = _config;
        }

        public ActionResult<List<ContactDTO>?> GetContacts(string json)
        {
            try
            {
                var receiversId = JsonConvert.DeserializeObject<List<string>>(json);                

                var contactsDTO = new List<ContactDTO>();

                foreach(var receiverId in receiversId)
                {
                    var contactDTO = Contact(receiverId);

                    contactsDTO.Add(contactDTO);
                }

                return Ok(contactsDTO);
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }

        public ActionResult<List<ContactDTO>?> GetContact(string receiverId)
        {
            try
            {
                var contactDTO = Contact(receiverId);

                return Ok(contactDTO);
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }

        private ContactDTO? Contact(string receiverId)
        {
            try
            {
                var chatAPI = new ChatAPI(Config);

                Authentication();

                var chatModel = new ChatGetConversationModel(receiverId, 0, 10);
                var chatsDTO = chatAPI.GetChatConversation(chatModel, Token).Result;

                var userAPI = new UserAPI(Config);

                if (chatsDTO != null && chatsDTO.Count > 0)
                {
                    var jsonUser = _privateController.GetSession("User");
                    var userDTO = JsonConvert.DeserializeObject<UserDTO>(jsonUser);

                    AuthenticationAdmin();

                    var receiverUserDTO = userAPI.GetUserById(receiverId, TokenAdmin).Result;

                    var contactDTO = new ContactDTO()
                    {
                        Id = receiverId,
                        ReceiverName = receiverUserDTO.Name,
                        UndisplayedMessages = 0
                    };

                    foreach (var chatDTO in chatsDTO)
                    {
                        if (userDTO.Id == chatDTO.UserId)
                        {
                            if (contactDTO.Content == null)
                            {
                                contactDTO.Content = chatDTO.Content;
                                contactDTO.Created = chatDTO.Timestamp;
                                contactDTO.isReceiverMessage = false;
                            }

                            break;
                        }
                        else
                        {
                            if (chatDTO.Viewed == null)
                            {
                                contactDTO.Content = chatDTO.Content;
                                contactDTO.Created = chatDTO.Timestamp;
                                contactDTO.isReceiverMessage = true;
                                contactDTO.UndisplayedMessages += 1;
                            }
                            else
                            {
                                if (contactDTO.UndisplayedMessages == 0)
                                {
                                    contactDTO.Content = chatDTO.Content;
                                    contactDTO.Created = chatDTO.Timestamp;
                                    contactDTO.isReceiverMessage = true;
                                }

                                break;
                            }
                        }
                    }

                    return contactDTO;
                }
                else
                {
                    AuthenticationAdmin();

                    var userDTO = userAPI.GetUserById(receiverId, TokenAdmin).Result;

                    var contactDTO = new ContactDTO();
                    contactDTO.Id = receiverId;
                    contactDTO.ReceiverName = userDTO.Name;

                    return contactDTO;
                }
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public ActionResult<List<ChatDTO>?> GetConversation(string json)
        {
            try
            {
                var chatModel = JsonConvert.DeserializeObject<ChatGetConversationModel>(json);

                Authentication();

                var chatAPI = new ChatAPI(Config);
                var chatsDTO = chatAPI.GetChatConversation(chatModel, Token).Result;

                chatsDTO = chatsDTO.OrderBy(x => x.Timestamp).ToList();

                return Ok(chatsDTO);
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }

        public ActionResult<List<ChatDTO>?> VisualizeMessages(string receiverId)
        {
            try
            {
                Authentication();

                var chatAPI = new ChatAPI(Config);
                var chatsDTO = chatAPI.VisualizeMessages(receiverId, Token).Result;

                return Ok(chatsDTO);
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }

        public ActionResult<bool> InsertChat(string json)
        {
            try
            {
                var chatModel = JsonConvert.DeserializeObject<ChatInsertModel>(json);
                chatModel.Timestamp = DateTime.Now;

                Authentication();

                var chatAPI = new ChatAPI(Config);
                var result = chatAPI.InsertChat(chatModel, Token).Result;

                return Ok(result);
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }
    }
}
