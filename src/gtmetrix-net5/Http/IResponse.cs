using System.Net;

namespace GTmetrix5.Http
{
    public interface IApiResponse
    {
        bool Success { get; }
        HttpStatusCode StatusCode { get; }
        string Error { get; }
    }
}
