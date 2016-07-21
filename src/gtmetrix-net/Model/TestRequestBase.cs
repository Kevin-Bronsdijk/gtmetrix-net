
using Newtonsoft.Json;

namespace GTmetrix.Model
{
    public abstract class TestRequestBase : ITestRequest, IRequest
    {
        [JsonProperty("url")]
        internal string Url { get; set; }
    }
}