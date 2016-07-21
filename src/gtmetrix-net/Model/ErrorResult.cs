using Newtonsoft.Json;

namespace GTmetrix.Model
{
    public class ErrorResult
    {
        [JsonProperty("message")]
        public string Error { get; set; }
    }
}