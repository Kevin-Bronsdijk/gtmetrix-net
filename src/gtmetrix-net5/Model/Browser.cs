using Newtonsoft.Json;

namespace GTmetrix5.Model
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

        [JsonProperty("features")]
        public Features Features { get; set; }
    }
}
