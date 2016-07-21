namespace GTmetrix.Http
{
    public interface IApiResponse<out TResult> : IApiResponse
    {
        TResult Body { get; }
    }
}