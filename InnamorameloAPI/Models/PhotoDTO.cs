namespace InnamorameloAPI.Models
{
    public class PhotoDTO : Photo
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public byte[]? Bytes { get; set; }
        public string? Extension { get; set; }

        public PhotoDTO() { }
        public PhotoDTO(string? userId, byte[]? bytes, string? name, string? extension, int position)
        {
            UserId = userId;
            Bytes = bytes;
            Name = name;
            Extension = extension;
            Position = position;
        }
    }
}
