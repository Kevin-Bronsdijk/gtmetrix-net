using Newtonsoft.Json;

namespace GTmetrix5.Model
{
    public class TestResult
    {
        internal TestResult()
        {
        }

        //Todo: Add test id

        /// <summary>
        /// The current state of the test.
        /// </summary>
        [JsonProperty("state")]
        public ResultStates State { get; set; }

        /// <summary>
        /// The error message.
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("results")]
        public Results Results { get; set; }

        ///// <summary>
        ///// URLs of downloadable resources.
        ///// </summary>
        [JsonProperty("resources")]
        public Resources Resources { get; set; }
    }
}
