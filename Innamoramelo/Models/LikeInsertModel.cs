namespace Innamoramelo.Models
{
    public class LikeInsertModel
    {
        public string? UserId { get; set; }
        public string? ReceiverId { get; set; }
        public DateTime? Created { get; set; }
        public bool? IsLiked { get; set; }
    }
}
