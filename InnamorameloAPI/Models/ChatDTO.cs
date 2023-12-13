namespace InnamorameloAPI.Models
{
    public class ChatDTO : Chat
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? ReceiverId { get; set; }
    }
}
