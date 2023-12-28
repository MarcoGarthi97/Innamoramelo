namespace Innamoramelo.Models
{
    public class ChatGetConversationModel
    {
        public string? ReceiverId { get; set; }
        public int? Skip { get; set; }
        public int? Limit { get; set; }

        public ChatGetConversationModel() { }
        public ChatGetConversationModel(string? receiverId)
        {
            ReceiverId = receiverId;
        }
        public ChatGetConversationModel(string? receiverId, int? skip, int? limit)
        {
            ReceiverId = receiverId;
            Skip = skip;
            Limit = limit;
        }
    }
}
