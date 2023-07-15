using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Plugin.Device.Geolocation.Geocoding
{
    public class Position
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Address
    {
        public string adminDistrict { get; set; }
        public string countryRegion { get; set; }
        public string formattedAddress { get; set; }
        public string locality { get; set; }
        public string adminDistrict2 { get; set; }
    }

    public class GeocodePoint
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
        public string calculationMethod { get; set; }
        public List<string> usageTypes { get; set; }
    }

    public class Resource
    {
        public string __type { get; set; }
        public List<double> bbox { get; set; }
        public string name { get; set; }
        public Position point { get; set; }
        public Address address { get; set; }
        public string confidence { get; set; }
        public string entityType { get; set; }
        public List<GeocodePoint> geocodePoints { get; set; }
        public List<string> matchCodes { get; set; }
    }

    public class ResourceSet
    {
        public int estimatedTotal { get; set; }
        public List<Resource> resources { get; set; }
    }

    public class GeocodeResult
    {
        public string authenticationResultCode { get; set; }
        public string brandLogoUri { get; set; }
        public string copyright { get; set; }
        public List<ResourceSet> resourceSets { get; set; }
        public int statusCode { get; set; }
        public string statusDescription { get; set; }
        public string traceId { get; set; }
    }

    public class GeocodeClient
    {

        public string ApiKey { get; set; }

        public GeocodeClient(string apikey)
        {
            ApiKey = apikey;
        }
        
        private async Task<GeocodeResult> GetGeocodeResult(string url)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage json;
            try
            {
                json = await client.GetAsync(url);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return null;
            }

            if (json.IsSuccessStatusCode)
            {
                var content = await json.Content.ReadAsStringAsync();
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<GeocodeResult>(content);
                return result;
            }
            return null;
        }

        public async Task<IList<Geolocation>> GetPointByAddress(string address)
        {
            IList<Geolocation> points = new List<Geolocation>();

            var url = $"http://dev.virtualearth.net/REST/v1/Locations?q={address}&key={ApiKey}";
            var result = await GetGeocodeResult(url);

            if (result == null) return points;

            if (result.statusCode == 200)
            {
                var resourcesets = result.resourceSets;

                if (resourcesets.Count > 0)
                {
                    var resources = resourcesets[0].resources;

                    if (resources.Count > 0)
                    {
                        foreach (var resource in resources)
                        {
                            var addressfound = resource.address;
                            var direction = new Direction();
                            if (address != null)
                            {
                                direction.Country = addressfound.countryRegion;
                                direction.State = addressfound.adminDistrict;
                                direction.City = addressfound.adminDistrict2;
                                direction.Locality = addressfound.locality;
                            }

                            if (resource.point != null)
                            {
                                if (resource.point.type == "Point")
                                {
                                    var latitud = resource.point.coordinates[0];
                                    var longitud = resource.point.coordinates[1];

                                    points.Add(new Geolocation
                                    {
                                        Latitude = latitud,
                                        Longitude = longitud,
                                        Direction = direction
                                    });
                                }
                            }
                        }
                    }
                }
            }

            return points;
        }

        public async Task<IList<string>> GetAddressByPoint(Geolocation point)
        {
            var url = $"http://dev.virtualearth.net/REST/v1/Locations/{point.Latitude},{point.Longitude}?key={ApiKey}";
            var result = await GetGeocodeResult(url);
            IList<string> address = new List<string>();
            if(result != null)
            {
                if(result.statusCode == 200)
                {
                    if(result.resourceSets != null)
                    {
                        var sets = result.resourceSets[0];

                        if(sets.resources != null)
                        {
                            foreach (var resource in sets.resources)
                            {
                                address.Add(resource.name);
                            }
                        }
                    }
                }
            }
            return address;
        }
    }
}
