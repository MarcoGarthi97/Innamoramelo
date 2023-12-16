using Newtonsoft.Json;
using RestSharp;

namespace InnamorameloAPI.Models
{
    public class GeoDBAPI : RapidAPI
    {
        private string UrlAPI = "https://wft-geo-db.p.rapidapi.com/v1/geo/";
        internal List<GeoDTO>? GetPlace(string filter)
        {
            try
            {
                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);

                var request = new RestRequest("cities?countryIds=IT&namePrefix=" + filter + "&languageCode=IT", Method.Get);
                request.AddHeader("X-RapidAPI-Key", ApiKey);
                request.AddHeader("X-RapidAPI-Host", "wft-geo-db.p.rapidapi.com");

                RestResponse response = client.Execute(request);

                var content = response.Content.Substring(response.Content.IndexOf("data") + 6, response.Content.IndexOf("metadata") - 8 - response.Content.IndexOf("data"));

                var geosDTO = JsonConvert.DeserializeObject<List<GeoDTO>?>(content);
                geosDTO = geosDTO.DistinctBy(x => x.Name).OrderBy(x => x.Name).ToList();

                return geosDTO;
            }
            catch(Exception ex)
            {

            }

            return null;
        }

        internal int? GetPlaceDistance(int idPlace1, int idPlace2)
        {
            try
            {
                var options = new RestClientOptions(UrlAPI)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);

                var request = new RestRequest("places/" + idPlace1 + "/distance?distanceUnit=KM&toPlaceId=" + idPlace2, Method.Get);
                request.AddHeader("X-RapidAPI-Key", ApiKey);
                request.AddHeader("X-RapidAPI-Host", "wft-geo-db.p.rapidapi.com");

                RestResponse response = client.Execute(request);

                var d = double.Parse(response.Content.Substring(8, response.Content.IndexOf("}") - 8).Replace(".", ","));
                var s = String.Format("{0:0}", d);
                var distance = int.Parse(s);

                return distance;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}
