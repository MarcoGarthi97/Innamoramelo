namespace Innamoramelo.Models
{
    public class LikeDTO
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? ReceiverId { get; set; }
        public DateTime? Created { get; set; }
        public bool? IsLiked { get; set; }
    }
}
