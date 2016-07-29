using Newtonsoft.Json;

namespace GTmetrix.Model
{
    public class Browser
    {
        internal Browser()
        {
        }

        /// <summary>
        /// The browser ID.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The browser display name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The browser platform (desktop/android).
        /// </summary>
        [JsonProperty("platform")]
        public string Platform { get; set; }

        /// <summary>
        /// The browser device type (phone/tablet); if applicable.
        /// </summary>
        [JsonProperty("device")]
        public string Device { get; set; }

        /// <summary>
        /// The browser (firefox/chrome).
        /// </summary>
        [JsonProperty("browser")]
        public string BrowserName { get; set; }

        /// <summary>
        /// Whether this browser supports AdBlock.
        /// </summary>
        [JsonProperty("features.adblock")]
        public bool Adblock { get; set; }

        /// <summary>
        /// Supports specifying cookies.
        /// </summary>
        [JsonProperty("features.cookies")]
        public bool Cookies { get; set; }

        /// <summary>
        /// Supports URL whitelist/blacklists.
        /// </summary>
        [JsonProperty("features.filtering")]
        public bool Filtering { get; set; }

        /// <summary>
        /// Supports HTTP authentication.
        /// </summary>
        [JsonProperty("features.http_auth")]
        public bool HttpAuth { get; set; }

        /// <summary>
        /// Supports connection throttling.
        /// </summary>
        [JsonProperty("features.throttle")]
        public bool Throttle { get; set; }

        /// <summary>
        /// Browser supports video generation
        /// </summary>
        [JsonProperty("features.video")]
        public bool Video { get; set; }
    }
}