namespace InnamorameloAPI.Models
{
    public class LocationAPI
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public LocationAPI() { }
        public LocationAPI(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
