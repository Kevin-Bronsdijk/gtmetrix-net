
using Newtonsoft.Json;

namespace GTmetrix.Model
{
    public class SubmitTestResult
    {
        internal SubmitTestResult()
        {
        }

        /// <summary>
        /// URL to use to poll test state.
        /// </summary>
        [JsonProperty("poll_state_url")]
        public string PollStateUrl { get; set; }

        /// <summary>
        /// The test ID.
        /// </summary>
        [JsonProperty("test_id")]
        public string TestId { get; set; }
    }
}