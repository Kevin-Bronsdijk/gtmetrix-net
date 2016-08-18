using Newtonsoft.Json;

namespace GTmetrix.Model
{
    public class ErrorResult
    {
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}