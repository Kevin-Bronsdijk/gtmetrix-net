
using Newtonsoft.Json;

namespace GTmetrix.Model
{
    public class TestResult
    {
        internal TestResult()
        {
        }

        /// <summary>
        /// The current state of the test.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        /// The error message.
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }

        //[JsonProperty("results")]
        //public string Results { get; set; }

        /// <summary>
        /// The URL to the GTmetrix report.
        /// </summary>
        [JsonProperty("results.report_url")]
        public string ReportUrl { get; set; }

        /// <summary>
        /// PageSpeed score.
        /// </summary>
        [JsonProperty("results.pagespeed_score")]
        public int PageSpeedScore { get; set; }

        /// <summary>
        /// YSlow score.
        /// </summary>
        [JsonProperty("results.yslow_score")]
        public int YSlowScore { get; set; }

        /// <summary>
        /// The HTML size (may be compressed).
        /// </summary>
        [JsonProperty("results.html_bytes")]
        public int HtmlInbytes { get; set; }

        /// <summary>
        /// The HTML load time (in milliseconds).
        /// </summary>
        [JsonProperty("results.html_load_time")]
        public int HtmlLoadTime { get; set; }

        /// <summary>
        /// The total page size.
        /// </summary>
        [JsonProperty("results.page_bytes")]
        public int PageBytes { get; set; }

        /// <summary>
        /// The page load time (in milliseconds)
        /// </summary>
        [JsonProperty("results.page_load_time")]
        public int PageLoadTime { get; set; }

        /// <summary>
        /// The number of page elements (# of resources)
        /// </summary>
        [JsonProperty("results.page_elements")]
        public int PageElements { get; set; }

        ///// <summary>
        ///// URLs of downloadable resources.
        ///// </summary>
        //[JsonProperty("resources")]
        //public string Resources { get; set; }

        /// <summary>
        /// URL to download the screenshot.
        /// </summary>
        [JsonProperty("resources.screenshot")]
        public string ScreenshotUrl { get; set; }

        /// <summary>
        /// URL to download the HAR file.
        /// </summary>
        [JsonProperty("resources.har")]
        public string HarUrl { get; set; }

        /// <summary>
        /// URL to download the PageSpeed beacon.
        /// </summary>
        [JsonProperty("resources.pagespeed")]
        public string PageSpeedUrl { get; set; }

        /// <summary>
        /// URL to download the PageSpeed optimized files.
        /// </summary>
        [JsonProperty("resources.pagespeed_files")]
        public string PageSpeedFiles { get; set; }

        /// <summary>
        /// URL to download the YSlow beacon.
        /// </summary>
        [JsonProperty("resources.yslow")]
        public string YslowUrl { get; set; }

        /// <summary>
        /// The URL to the GTmetrix report in PDF format
        /// </summary>
        [JsonProperty("resources.report_pdf")]
        public string PdfReportUrl { get; set; }

        /// <summary>
        /// The URL to the full GTmetrix report in PDF format
        /// </summary>
        [JsonProperty("resources.report_pdf_full")]
        public string DetailedPdfReportUrl { get; set; }

        /// <summary>
        /// The URL to the optional GTmetrix video in mp4 format
        /// </summary>
        [JsonProperty("resources.video")]
        public string VideoUrl { get; set; }
    }
}