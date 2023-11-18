namespace InnamorameloAPI.Models
{
    public class PhotoAPI
    {
        public string? NameFile { get; set; }
        public string? Path { get; set; }
        public byte[]? Bytes { get; set; }

        public PhotoAPI() { }
        public PhotoAPI(string? nameFile, string? path, byte[]? bytes)
        {
            NameFile = nameFile;
            Path = path;
            Bytes = bytes;
        }
    }
}
