namespace InnamorameloAPI.Models
{
    public class LikeDTO : Like
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? ReceiverId { get; set; }
    }
}
