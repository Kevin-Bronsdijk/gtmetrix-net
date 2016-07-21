using System.Net.Http;
using GTmetrix.Model;

namespace GTmetrix.Http
{
    internal interface IApiRequest
    {
        string Uri { get; set; }
        IRequest Body { get; set; }
        HttpMethod Method { get; set; }
    }
}