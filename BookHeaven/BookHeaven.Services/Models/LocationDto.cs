using Newtonsoft.Json;

namespace BookHeaven.Services.Models
{
    public class LocationDto
    {
        [JsonProperty("country_name")]
        public string Country { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }
    }
}
