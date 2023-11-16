namespace InnamorameloAPI.Models
{
    public class Photo
    {
        public string? NameFile { get; set; }
        public string? Path { get; set; }
        public byte[]? Bytes { get; set; }

        public Photo() { }
        public Photo(string? nameFile, string? path, byte[]? bytes)
        {
            NameFile = nameFile;
            Path = path;
            Bytes = bytes;
        }
    }
}
