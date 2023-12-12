namespace Innamoramelo.Models
{
    public class PhotoViewModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public int? Position { get; set; }

        public PhotoViewModel() { }
        public PhotoViewModel(string? id, string? name, int? position)
        {
            Id = id;
            Name = name;
            Position = position;
        }
    }
}
