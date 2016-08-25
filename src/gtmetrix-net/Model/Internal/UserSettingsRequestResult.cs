
using Newtonsoft.Json;

namespace GTmetrix.Model.Internal
{
    internal class UserSettingsRequestResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
