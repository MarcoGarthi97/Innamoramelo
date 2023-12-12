namespace Innamoramelo.Models
{
    public class ChatGetConversationModel
    {
        public string? ReceiverId { get; set; }
        public int? Skip { get; set; }
        public int? Limit { get; set; }
    }
}
