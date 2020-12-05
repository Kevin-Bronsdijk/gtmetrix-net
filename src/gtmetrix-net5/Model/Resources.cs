using Newtonsoft.Json;

namespace GTmetrix5.Model
{
    public class Resources
    {
        /// <summary>
        /// URL to download the screenshot.
        /// </summary>
        [JsonProperty("screenshot")]
        public string ScreenshotUrl { get; set; }

        /// <summary>
        /// URL to download the HAR file.
        /// </summary>
        [JsonProperty("har")]
        public string HarUrl { get; set; }

        /// <summary>
        /// URL to download the PageSpeed beacon.
        /// </summary>
        [JsonProperty("pagespeed")]
        public string PageSpeedUrl { get; set; }

        /// <summary>
        /// URL to download the PageSpeed optimized files.
        /// </summary>
        [JsonProperty("pagespeed_files")]
        public string PageSpeedFiles { get; set; }

        /// <summary>
        /// URL to download the YSlow beacon.
        /// </summary>
        [JsonProperty("yslow")]
        public string YslowUrl { get; set; }

        /// <summary>
        /// The URL to the GTmetrix report in PDF format
        /// </summary>
        [JsonProperty("report_pdf")]
        public string PdfReportUrl { get; set; }

        /// <summary>
        /// The URL to the full GTmetrix report in PDF format
        /// </summary>
        [JsonProperty("report_pdf_full")]
        public string DetailedPdfReportUrl { get; set; }

        /// <summary>
        /// The URL to the optional GTmetrix video in mp4 format
        /// </summary>
        [JsonProperty("video")]
        public string VideoUrl { get; set; }
    }
}
