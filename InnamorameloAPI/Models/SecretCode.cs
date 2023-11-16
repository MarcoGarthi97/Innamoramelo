namespace InnamorameloAPI.Models
{
    public class SecretCode
    {
        public string? Code { get; set; }
        public DateTime? Created { get; set; }

        public SecretCode() { }
        public SecretCode(string? code, DateTime? created)
        {
            Code = code;
            Created = created;
        }
    }
}
