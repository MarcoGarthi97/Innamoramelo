namespace InnamorameloAPI.Models
{
    public class User
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? Birthday { get; set; }
        public bool? IsActive { get; set; }
        public bool? CreateProfile { get; set; }
    }
}