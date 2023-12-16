namespace InnamorameloAPI.Models
{
    public class Profile
    {
        public string? Gender { get; set; }
        public string? SexualOrientation { get; set; }
        public string[]? LookingFor { get; set; }
        public string? Education { get; set; }
        public string? Job { get; set; }
        public string? Bio { get; set; }
        public List<string>? Passions { get; set; }
        public GeoDTO? Location { get; set; }
        public int? RangeKm { get; set; }
    }
}
