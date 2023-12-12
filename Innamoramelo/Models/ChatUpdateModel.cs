namespace Innamoramelo.Models
{
    public class ChatUpdateModel
    {
        public string? Id { get; set; }
        public string? ReceiverId { get; set; }
        public string? Content { get; set; }
        public DateTime? Viewed { get; set; }
    }
}
