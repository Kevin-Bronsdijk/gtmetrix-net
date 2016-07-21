
using Newtonsoft.Json;

namespace GTmetrix.Model
{
    public class TestResult
    {
        internal TestResult()
        {
        }

        //Todo: Include
        //resources URLs of downloadable resources.Will be an empty object until state == "completed".	hash
        //resources.pagespeed_files URL to download the PageSpeed optimized files. The files are combined into a single tar file.   string
        //resources.yslow URL to download the YSlow beacon    string
        //resources.report_pdf The URL to the GTmetrix report in PDF format    string
        //resources.report_pdf_full The URL to the full GTmetrix report (includes recommendation details) in PDF format string
        //resources.video The URL to the optional GTmetrix video in mp4 format    string
        

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        //[JsonProperty("results")]
        //public string Results { get; set; }

        [JsonProperty("results.report_url")]
        public string ReportUrl { get; set; }

        [JsonProperty("results.pagespeed_score")]
        public int PageSpeedScore { get; set; }

        [JsonProperty("results.yslow_score")]
        public int YSlowScore { get; set; }

        [JsonProperty("results.html_bytes")]
        public int HtmlInbytes { get; set; }

        [JsonProperty("results.html_load_time")]
        public int HtmlLoadTime { get; set; }

        [JsonProperty("results.page_bytes")]
        public int PageBytes { get; set; }

        [JsonProperty("results.page_load_time")]
        public int PageLoadTime { get; set; }

        [JsonProperty("results.page_elements")]
        public int PageElements { get; set; }

        [JsonProperty("resources.screenshot")]
        public string ScreenshotUrl { get; set; }

        [JsonProperty("resources.har")]
        public string HarUrl { get; set; }

        [JsonProperty("resources.pagespeed")]
        public string PagespeedUrl { get; set; }
    }
}