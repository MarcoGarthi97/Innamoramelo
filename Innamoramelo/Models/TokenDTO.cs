namespace Innamoramelo.Models
{
    public class TokenDTO
    {
        public string Bearer { get; set; }
        public DateTime Expires { get; set; }
        public TokenDTO() { }
        public TokenDTO(string bearer, DateTime expires)
        {
            Bearer = bearer;
            Expires = expires;
        }
    }
}
