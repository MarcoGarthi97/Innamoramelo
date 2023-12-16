namespace Innamoramelo.Models
{
    public class ProfileViewModel
    {
        public string? Gender { get; set; }
        public string? SexualOrientation { get; set; }
        public string? LookingFor { get; set; }
        public string? School { get; set; }
        public string? Work { get; set; }
        public string? Bio { get; set; }
        public List<string>? Passions { get; set; }
        public GeoDTO? Location { get; set; }
        public int? RangeKm { get; set; }
    }
}
