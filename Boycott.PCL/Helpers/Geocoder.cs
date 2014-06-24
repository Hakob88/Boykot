using Boycott.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Boycott.PCL.Helpers
{
    #region Geocoder Models

    public class GeoCoordinate
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Viewport
    {
        public GeoCoordinate northeast { get; set; }
        public GeoCoordinate southwest { get; set; }
    }

    public class Geometry
    {
        public GeoCoordinate location { get; set; }
        public string location_type { get; set; }
        public Viewport viewport { get; set; }
    }

    public class AddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }

    public class GeoData
    {
        public List<AddressComponent> address_components { get; set; }
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public bool partial_match { get; set; }
        public List<string> types { get; set; }
    }

    public class GeocoderResponse
    {
        public List<GeoData> results { get; set; }
        public string status { get; set; }
    }

    #endregion

    class Geocoder
    {
        private static readonly string GOOGLE_MAPS_API_URL = "http://maps.google.com/maps/api/geocode/json?address=";
        private static readonly string GOOGLE_MAPS_API_DEFAULT_PARAMS = "&region=hy&sensor=false";

        private static HttpClient _downloader = new HttpClient();

        public static async Task<GeocoderResponse> GeocodeAsync(string address)
        {
            var responseData = new GeocoderResponse();
            string requestUrl = GOOGLE_MAPS_API_URL + address + GOOGLE_MAPS_API_DEFAULT_PARAMS;

            try
            {
                string response = await _downloader.GetStringAsync(requestUrl);

                var json = new Helpers.JSONParser<GeocoderResponse>();
                responseData = json.parse(response);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Geocoding on address \"" + address + "\" failed: " + ex.Message);
            }

            return responseData;
        }
    }



}
