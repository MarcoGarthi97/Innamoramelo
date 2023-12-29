using Newtonsoft.Json;
using RestSharp;

namespace Innamoramelo.Models
{
    public class ChatAPI
    {
        private string UrlAPI { get; set; }
        public ChatAPI(IConfiguration config)
        {
            UrlAPI = config["urlAPI"] + "/Chat/";
        }

        internal async Task<List<ChatDTO>?> GetChatConversation(ChatGetConversationModel chatModel, string token)
        {
            try
            {
                string json = JsonConvert.SerializeObject(chatModel);

                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);

                var request = new RestRequest("GetChatConversation", Method.Post);
                request.AddHeader("Authorization", "Bearer " + token);

                var body = json;
                request.AddStringBody(body, DataFormat.Json);

                RestResponse response = await client.ExecuteAsync(request);

                var chatsDTO = JsonConvert.DeserializeObject<List<ChatDTO>>(response.Content);
                return chatsDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        internal async Task<List<ChatDTO>?> VisualizeMessages(string receiverId, string token)
        {
            try
            {
                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);

                var request = new RestRequest("VisualizeMessages?receiverId=" + receiverId, Method.Post);
                request.AddHeader("Authorization", "Bearer " + token);

                RestResponse response = await client.ExecuteAsync(request);

                var chatsDTO = JsonConvert.DeserializeObject<List<ChatDTO>>(response.Content);
                return chatsDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        internal async Task<bool?> InsertChat(ChatInsertModel chatModel, string token)
        {
            try
            {
                string json = JsonConvert.SerializeObject(chatModel);

                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);

                var request = new RestRequest("InsertChat", Method.Post);
                request.AddHeader("Authorization", "Bearer " + token);

                var body = json;
                request.AddStringBody(body, DataFormat.Json);

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
