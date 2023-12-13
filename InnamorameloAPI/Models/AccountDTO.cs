namespace InnamorameloAPI.Models
{
    public class AccountDTO : Account
    {
        public string Id { get; set; }
        public AccountDTO() { }
        public AccountDTO(string? username, string? password, string? level)
        {
            Username = username;
            Password = password;
            Level = level;
        }
    }
}
