namespace GTmetrix5.Http
{
    public interface IApiResponse<out TResult> : IApiResponse
    {
        TResult Body { get; }
    }
}
