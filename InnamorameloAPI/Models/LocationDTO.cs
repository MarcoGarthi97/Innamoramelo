namespace InnamorameloAPI.Models
{
    public class LocationDTO
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public LocationDTO() { }
        public LocationDTO(string name, double latitude, double longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
