using GTmetrix5.Logic;
using GTmetrix5.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GTmetrix5.Http
{
    public class Connection : IDisposable
    {
        private readonly string _apiKey;
        private readonly string _email;
        private readonly Uri _ApiUrl = new("https://gtmetrix.com/api/");
        private HttpClient _client;
        private JsonSerializerSettings _serializerSettings;

        internal Connection(string apiKey, string email, HttpMessageHandler handler)
        {
            _client = new HttpClient(handler) { BaseAddress = _ApiUrl };
            _client.DefaultRequestHeaders.Add("Accept", "application/json");

            ConfigureSerialization();

            _apiKey = apiKey;
            _email = email;

            SetAuthenticationHeader();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public static Connection Create(string apiKey, string username, IWebProxy proxy = null)
        {
            apiKey.ThrowIfNullOrEmpty(nameof(apiKey));
            username.ThrowIfNullOrEmpty(nameof(username));

            var handler = new HttpClientHandler { Proxy = proxy };
            return new Connection(apiKey, username, handler);
        }

        internal async Task<IApiResponse<TResponse>> Execute<TResponse>(ApiRequest apiRequest,
            CancellationToken cancellationToken)
        {
            using var requestMessage = new HttpRequestMessage(apiRequest.Method, apiRequest.Uri);
            if (!(apiRequest.Body is NoInstructionsRequest) && apiRequest.Body != null)
                requestMessage.Content = new FormUrlEncodedContent(apiRequest.Body.GetPostData());

            using var responseMessage = await _client.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
            return await BuildResponse<TResponse>(responseMessage, cancellationToken).ConfigureAwait(false);
        }

        internal async Task<IApiResponse<byte[]>> Download(ApiRequest apiRequest, CancellationToken cancellationToken)
        {
            using var requestMessage = new HttpRequestMessage(apiRequest.Method, apiRequest.Uri);
            using var responseMessage = await _client.SendAsync(requestMessage,
                HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            var response = new ApiResponse<byte[]>
            {
                StatusCode = responseMessage.StatusCode,
                Success = responseMessage.IsSuccessStatusCode
            };

            if (responseMessage.IsSuccessStatusCode)
            {
                var httpStream = await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
                response.Body = Helper.ReadFully(httpStream);
            }

            return response;
        }

        private void ConfigureSerialization()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCaseExceptDictionaryKeysResolver(),
                Converters = new List<JsonConverter> { new StringEnumConverter { CamelCaseText = true } },
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        private void SetAuthenticationHeader()
        {
            var byteArray = Encoding.ASCII.GetBytes($"{_email}:{_apiKey}");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        private async Task<T> ParseResponseMessageToObject<T>(HttpResponseMessage responseMessage, CancellationToken cancellationToken)
        {
            using var stream = await responseMessage.Content.ReadAsStreamAsync();
            //Todo: Implement cancellationToken support
            return JsonConvert.DeserializeObject<T>(new StreamReader(stream).ReadToEnd(), _serializerSettings);
        }

        private async Task<IApiResponse<TResponse>> BuildResponse<TResponse>(HttpResponseMessage message, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<TResponse>
            {
                StatusCode = message.StatusCode,
                Success = message.IsSuccessStatusCode
            };

            if (message.IsSuccessStatusCode)
            {
                response.Body = await ParseResponseMessageToObject<TResponse>(message, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                var errorResponse =
                    await ParseResponseMessageToObject<ErrorResult>(message, cancellationToken).ConfigureAwait(false);

                if (errorResponse != null)
                    response.Error = errorResponse.Error;
            }

            return response;
        }

        ~Connection()
        {
            Dispose(false);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_client != null)
                {
                    _client.Dispose();
                    _client = null;
                }
            }
        }
    }
}
