
using Newtonsoft.Json;

namespace GTmetrix.Model
{
    public class SubmitTestResult
    {
        internal SubmitTestResult()
        {
        }

        [JsonProperty("poll_state_url")]
        public string PollStateUrl { get; set; }

        [JsonProperty("test_id")]
        public string TestId { get; set; }
    }
}