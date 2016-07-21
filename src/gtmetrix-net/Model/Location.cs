using Newtonsoft.Json;
using System.Collections.Generic;

namespace GTmetrix.Model
{
    public class Location 
    {
        internal Location()
        {
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("default")]
        public bool IsDefault { get; set; }

        [JsonProperty("browsers")]
        public List<string> Browsers { get; set; }
    }
}