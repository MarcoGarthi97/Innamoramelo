namespace InnamorameloAPI.Models
{
    public class SecretCodeDTO : SecretCode
    {
        public string Id { get; set; }
        public string IdUser { get; set; }

        public SecretCodeDTO() { }
        public SecretCodeDTO(string? code, DateTime? created)
        {
            Code = code;
            Created = created;
        }
    }
}
