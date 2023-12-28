namespace Innamoramelo.Models
{
    public class ContactDTO
    {
        public string? Id { get; set; }
        public string? ReceiverName { get; set; }
        public string? Content { get; set; }
        public DateTime? Created {  get; set; }
        public bool? isReceiverMessage { get; set; }
        public int? UndisplayedMessages { get; set; }
    }
}
