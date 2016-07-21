using System.Net;

namespace GTmetrix.Http
{
    public interface IApiResponse
    {
        bool Success { get; }
        HttpStatusCode StatusCode { get; }
        string Error { get; }
    }
}