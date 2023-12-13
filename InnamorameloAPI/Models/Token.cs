namespace InnamorameloAPI.Models
{
    public class Token
    {
        public string Bearer { get; set; }
        public DateTime Expires {  get; set; }
        public Token() { }
        public Token(string bearer, DateTime expires)
        {
            Bearer = bearer;
            Expires = expires;
        }
    }
}
