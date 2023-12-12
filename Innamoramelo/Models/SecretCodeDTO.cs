namespace Innamoramelo.Models
{
    public class SecretCodeDTO
    {
        public string Id { get; set; }
        public string IdUser { get; set; }
        public string? Code { get; set; }
        public DateTime? Created { get; set; }

        public SecretCodeDTO() { }
        public SecretCodeDTO(string? code, DateTime? created)
        {
            Code = code;
            Created = created;
        }
    }
}
