namespace Innamoramelo.Models
{
    public class LocationDTO
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public LocationDTO() { }
        public LocationDTO(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
