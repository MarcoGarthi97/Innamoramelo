namespace Innamoramelo.Models
{
    public class PhotoDTO
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public byte[]? Bytes { get; set; }
        public string? Extension { get; set; }
        public string? Name { get; set; }
        public int? Position { get; set; }

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
