namespace Innamoramelo.Models
{
    public class ChatDTO
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? ReceiverId { get; set; }
        public string? Content { get; set; }
        public DateTime? Timestamp { get; set; }
        public DateTime? Viewed { get; set; }
    }
}
