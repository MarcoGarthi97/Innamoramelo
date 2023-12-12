namespace Innamoramelo.Models
{
    public class AuthenticationDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public AuthenticationDTO() { }
        public AuthenticationDTO(string email)
        {
            Email = email;
        }
        public AuthenticationDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
