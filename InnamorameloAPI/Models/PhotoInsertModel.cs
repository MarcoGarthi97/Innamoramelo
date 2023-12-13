namespace InnamorameloAPI.Models
{
    public class PhotoInsertModel : Photo
    {
        public byte[]? Bytes { get; set; }
        public string? Extension { get; set; }
    }
}
