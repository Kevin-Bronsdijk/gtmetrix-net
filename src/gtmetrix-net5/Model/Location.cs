using Newtonsoft.Json;
using System.Collections.Generic;

namespace GTmetrix5.Model
{
    public class Location
    {
        internal Location()
        {
        }

        /// <summary>
        /// The location ID.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The location name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Is the default location.
        /// </summary>
        [JsonProperty("default")]
        public bool IsDefault { get; set; }

        /// <summary>
        /// List of browser ID's supported by this location.
        /// </summary>
        [JsonProperty("browsers")]
        public List<string> Browsers { get; set; }
    }
}
