namespace Innamoramelo.Models
{
    public class PhotoInsertModel
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public byte[]? Bytes { get; set; }
        public string? Extension { get; set; }
        public string? Name { get; set; }
        public int? Position { get; set; }
    }
}
