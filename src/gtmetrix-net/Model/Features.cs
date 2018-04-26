using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTmetrix.Model
{
    public class Features
    {
        /// <summary>
        /// Supports specifying cookies.
        /// </summary>
        [JsonProperty("cookies")]
        public bool Cookies { get; set; }

        /// <summary>
        /// Whether this browser supports AdBlock.
        /// </summary>
        [JsonProperty("adblock")]
        public bool AdBlock { get; set; }

        /// <summary>
        /// Supports HTTP authentication.
        /// </summary>
        [JsonProperty("http_auth")]
        public bool HttpAuth { get; set; }

        /// <summary>
        /// Browser supports video generation
        /// </summary>
        [JsonProperty("video")]
        public bool Video { get; set; }

        /// <summary>
        /// Supports URL whitelist/blacklists.
        /// </summary>
        [JsonProperty("filtering")]
        public bool Filtering { get; set; }

        /// <summary>
        /// Supports connection throttling.
        /// </summary>
        [JsonProperty("throttle")]
        public bool Throttle { get; set; }
    }
}
