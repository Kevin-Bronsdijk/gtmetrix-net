using Newtonsoft.Json;

namespace GTmetrix5.Model
{
    public class ErrorResult
    {
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
