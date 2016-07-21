using Newtonsoft.Json;

namespace GTmetrix.Model
{
    public class Status
    {
        internal Status()
        {
        }

        [JsonProperty("api_credits")]
        public int Credits { get; set; }

        [JsonProperty("api_topup")]
        public int Topup { get; set; }
    }
}