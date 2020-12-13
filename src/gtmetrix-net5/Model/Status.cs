using Newtonsoft.Json;

namespace GTmetrix5.Model
{
    public class Status
    {
        internal Status()
        {
        }

        /// <summary>
        /// Amount of API credits remaining.
        /// </summary>
        [JsonProperty("api_credits")]
        public int Credits { get; set; }

        /// <summary>
        /// Unix timestamp for next API topup.
        /// </summary>
        [JsonProperty("api_topup")]
        public int Topup { get; set; }
    }
}
