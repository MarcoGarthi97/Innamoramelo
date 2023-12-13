namespace InnamorameloAPI.Models
{
    public class LikeInsertModel : Like
    {
        public string? UserId { get; set; }
        public string? ReceiverId { get; set; }
    }
}
