namespace Innamoramelo.Models
{
    public class ProfileViewModel
    {
        public string? Gender { get; set; }
        public string? SexualOrientation { get; set; }
        public string[]? LookingFor { get; set; }
        public AgeDTO? Age { get; set; }
        public string? Education { get; set; }
        public string? Job { get; set; }
        public string? Bio { get; set; }
        public string[]? Passions { get; set; }
        public GeoDTO? Location { get; set; }
        public int? RangeKm { get; set; }
    }
}
