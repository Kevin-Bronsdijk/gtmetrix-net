using GTmetrix5.Logic;
using GTmetrix5.Model;
using GTmetrix5.Model.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GTmetrix5.Http
{
    /// <summary>
    /// This isn't part of the official API and therefore exposed separately.
    /// </summary>
    public class UserSettingsConnection : IDisposable
    {
        private readonly string _password;
        private readonly string _email;
        private readonly Uri _ApiUrl = new("https://gtmetrix.com/");
        private HttpClient _client;
        private JsonSerializerSettings _serializerSettings;
        private readonly CookieContainer _cookies = new();
        DateTime _lastSuccessfulAuthentication;
        private readonly int _authenticationTimeout = 5;

        internal UserSettingsConnection(string email, string password, HttpClientHandler handler)
        {
            handler.CookieContainer = _cookies;

            _client = new HttpClient(handler) { BaseAddress = _ApiUrl };
            _client.DefaultRequestHeaders.Add("Accept", "application/json");

            ConfigureSerialization();

            _password = password;
            _email = email;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public static UserSettingsConnection Create(string email, string password, IWebProxy proxy = null)
        {
            password.ThrowIfNullOrEmpty(nameof(password));
            email.ThrowIfNullOrEmpty(nameof(email));

            var handler = new HttpClientHandler { Proxy = proxy };
            return new UserSettingsConnection(email, password, handler);
        }

        internal async Task<IApiResponse<TResponse>> Execute<TResponse>(ApiRequest apiRequest, CancellationToken cancellationToken)
        {
            if (!IsAuthenticated())
            {
                var loginResults = await Login(cancellationToken);

                if (loginResults.Body == null || !loginResults.Body.Success)
                    return Helper.CreateFailedResponse<TResponse>("failed to login", HttpStatusCode.BadRequest);

                _lastSuccessfulAuthentication = DateTime.Now;
            }

            using var requestMessage = new HttpRequestMessage(apiRequest.Method, apiRequest.Uri);
            if (HasPostData(apiRequest))
            {
                requestMessage.Content = new FormUrlEncodedContent(apiRequest.Body.GetPostData());

                if (!string.IsNullOrEmpty(apiRequest.Referer))
                    requestMessage.Headers.Add("Referer", _ApiUrl + apiRequest.Referer);
            }

            using var responseMessage = await _client.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
            return await BuildResponse<TResponse>(responseMessage, cancellationToken).ConfigureAwait(false);
        }

        private async Task<IApiResponse<UserSettingsRequestResult>> Login(CancellationToken cancellationToken)
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, "login/");
            List<KeyValuePair<string, string>> keyValues = new();
            keyValues.Add(new KeyValuePair<string, string>("email", _email));
            keyValues.Add(new KeyValuePair<string, string>("password", _password));

            requestMessage.Content = new FormUrlEncodedContent(keyValues);

            using var responseMessage = await _client.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
            return await BuildResponse<UserSettingsRequestResult>(responseMessage, cancellationToken).ConfigureAwait(false);
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

        private bool IsAuthenticated()
        {
            if ((DateTime.Now - _lastSuccessfulAuthentication).TotalMinutes > _authenticationTimeout)
            {
                return false;
            }
            else
            {
                // Todo: Check cookies
                if (_cookies.Count == 0)
                {
                    return false;
                }
                return true;
            }
        }

        private bool HasPostData(ApiRequest apiRequest)
        {
            if (apiRequest.Method == HttpMethod.Post && !(apiRequest.Body is NoInstructionsRequest) && apiRequest.Body != null)
            {
                return true;
            }

            return false;
        }

        private async Task<T> ParseResponseMessageToObject<T>(HttpResponseMessage responseMessage, CancellationToken cancellationToken)
        {
            using var stream = await responseMessage.Content.ReadAsStreamAsync();
            //Todo: Implement cancellationToken support
            using StreamReader reader = new(stream);
            var result = reader.ReadToEnd();

            if (Helper.Html.IsHtml(result))
                return (T)Activator.CreateInstance(typeof(T), new object[] { result });
            else
                return JsonConvert.DeserializeObject<T>(result, _serializerSettings);
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
                var errorResponse = await ParseResponseMessageToObject<ErrorResult>(message, cancellationToken).ConfigureAwait(false);

                if (errorResponse != null)
                    response.Error = errorResponse.Error;
            }

            return response;
        }

        ~UserSettingsConnection()
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
