namespace InnamorameloAPI.Models
{
    public class PhotoInsertModel : Photo
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public byte[]? Bytes { get; set; }
        public string? Extension { get; set; }

        public PhotoInsertModel() { }
        public PhotoInsertModel(string? userId, byte[]? bytes, string? name, string? extension, int position)
        {
            UserId = userId;
            Bytes = bytes;
            Name = name;
            Extension = extension;
            Position = position;
        }
    }
}
