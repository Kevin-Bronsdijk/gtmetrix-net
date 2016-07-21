using Newtonsoft.Json;

namespace GTmetrix.Model
{
    public class Browser
    {
        internal Browser()
        {
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("device")]
        public string Device { get; set; }

        [JsonProperty("browser")]
        public string BrowserName { get; set; }

        [JsonProperty("features.adblock")]
        public bool Adblock { get; set; }

        [JsonProperty("features.cookies")]
        public bool Cookies { get; set; }

        [JsonProperty("features.filtering")]
        public bool Filtering { get; set; }

        [JsonProperty("features.http_auth")]
        public bool HttpAuth { get; set; }

        [JsonProperty("features.throttle")]
        public bool Throttle { get; set; }

        [JsonProperty("features.video")]
        public bool Video { get; set; }
    }
}