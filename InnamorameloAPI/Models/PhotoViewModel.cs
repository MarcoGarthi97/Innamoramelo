namespace InnamorameloAPI.Models
{
    public class PhotoViewModel : Photo
    {
        public string? Id { get; set; }

        public PhotoViewModel() { }
        public PhotoViewModel(string? id, string? name, int? position)
        {
            Id = id;
            Name = name;
            Position = position;
        }
    }
}
