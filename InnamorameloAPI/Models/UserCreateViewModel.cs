namespace InnamorameloAPI.Models
{
    public class UserCreateViewModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public DateTime? Birthday { get; set; }
    }
}