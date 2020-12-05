using GTmetrix5.Model;
using System.Net.Http;

namespace GTmetrix5.Http
{
    internal interface IApiRequest
    {
        string Uri { get; set; }
        IRequest Body { get; set; }
        HttpMethod Method { get; set; }
    }
}
