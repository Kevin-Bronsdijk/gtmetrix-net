using Newtonsoft.Json;

namespace GTmetrix.Model
{
    public class Results
    {
        /// <summary>
        /// The URL to the GTmetrix report.
        /// </summary>
        [JsonProperty("report_url")]
        public string ReportUrl { get; set; }

        /// <summary>
        /// PageSpeed score.
        /// </summary>
        [JsonProperty("pagespeed_score")]
        public int PageSpeedScore { get; set; }

        /// <summary>
        /// YSlow score.
        /// </summary>
        [JsonProperty("yslow_score")]
        public int YSlowScore { get; set; }

        /// <summary>
        /// The HTML size (may be compressed).
        /// </summary>
        [JsonProperty("html_bytes")]
        public int HtmlInbytes { get; set; }

        /// <summary>
        /// The HTML load time (in milliseconds).
        /// </summary>
        [JsonProperty("html_load_time")]
        public int HtmlLoadTime { get; set; }

        /// <summary>
        /// The total page size.
        /// </summary>
        [JsonProperty("page_bytes")]
        public int PageBytes { get; set; }

        /// <summary>
        /// The page load time (in milliseconds)
        /// </summary>
        [JsonProperty("page_load_time")]
        public int PageLoadTime { get; set; }

        /// <summary>
        /// The number of page elements (# of resources)
        /// </summary>
        [JsonProperty("page_elements")]
        public int PageElements { get; set; }

        /// <summary>
        /// Fully loaded time
        /// </summary>
        [JsonProperty("fully_loaded_time")]
        public int FullyLoadedTime { get; set; }
    }
}
