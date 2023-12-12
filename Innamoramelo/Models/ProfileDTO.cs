namespace Innamoramelo.Models
{
    public class ProfileDTO
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? Gender { get; set; }
        public string? SexualOrientation { get; set; }
        public string? LookingFor { get; set; }
        public string? School { get; set; }
        public string? Work { get; set; }
        public string? Bio { get; set; }
        public List<string>? Passions { get; set; }
        public LocationDTO? Location { get; set; }
        public int? RangeKm { get; set; }
    }
}
