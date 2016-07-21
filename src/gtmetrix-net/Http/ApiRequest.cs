using System.Net.Http;
using GTmetrix.Model;

namespace GTmetrix.Http
{
    internal class ApiRequest : IApiRequest
    {
        public ApiRequest(IRequest body, string uri, HttpMethod method)
        {
            Uri = uri;
            Method = method;
            Body = body;
        }

        public string Uri { get; set; }
        public HttpMethod Method { get; set; }
        public IRequest Body { get; set; }
    }
}