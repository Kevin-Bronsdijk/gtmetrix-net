using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GTmetrix.Logic;
using GTmetrix.Model;
using GTmetrix.Model.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GTmetrix.Http
{
    /// <summary>
    /// This is not part of the official API and therefore exposed separately.
    /// </summary>
    public class UserSettingsConnection : IDisposable
    {
        private readonly string _password;
        private readonly string _email;
        private readonly Uri _ApiUrl = new Uri("https://gtmetrix.com/");
        private HttpClient _client;
        private JsonSerializerSettings _serializerSettings;
        CookieContainer _cookies = new CookieContainer();

        internal UserSettingsConnection(string email, string password, HttpClientHandler handler)
        {
            handler.CookieContainer = _cookies;

            _client = new HttpClient(handler) {BaseAddress = _ApiUrl};
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
            password.ThrowIfNullOrEmpty("password");
            email.ThrowIfNullOrEmpty("email");

            var handler = new HttpClientHandler {Proxy = proxy};
            return new UserSettingsConnection(email, password, handler);
        }

        internal async Task<IApiResponse<TResponse>> Execute<TResponse>(ApiRequest apiRequest, CancellationToken cancellationToken)
        {
            var loginResults = await Login(cancellationToken);

            if (loginResults.Body != null && loginResults.Body.Success)
            {
                using (var requestMessage = new HttpRequestMessage(apiRequest.Method, apiRequest.Uri))
                {
                    if (!(apiRequest.Body is NoInstructionsRequest) && apiRequest.Body != null)
                    {
                        var content = new FormUrlEncodedContent(apiRequest.Body.GetPostData());
                        requestMessage.Content = content;
                    }

                    using (var responseMessage = await _client.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false))
                    {
                        var test = await responseMessage.Content.ReadAsStringAsync();

                        return await BuildResponse<TResponse>(responseMessage, cancellationToken).ConfigureAwait(false);
                    }
                }
            }
            else
            {
                return Helper.CreateFailedResponse<TResponse>("failed to login", HttpStatusCode.BadRequest);
            }
        }

        private async Task<IApiResponse<UserSettingsRequestResult>> Login(CancellationToken cancellationToken)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, "login/"))
            {
                List<KeyValuePair<string, string>> keyValues = new List<KeyValuePair<string, string>>();
                keyValues.Add(new KeyValuePair<string, string>("email", _email));
                keyValues.Add(new KeyValuePair<string, string>("password", _password));

                var content = new FormUrlEncodedContent(keyValues);
                requestMessage.Content = content;

                using (var responseMessage = await _client.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false))
                {
                    // var test = await responseMessage.Content.ReadAsStringAsync();

                    return await BuildResponse<UserSettingsRequestResult>(responseMessage, cancellationToken).ConfigureAwait(false);
                }
            }
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

        private async Task<T> ParseResponseMessageToObject<T>(HttpResponseMessage responseMessage, CancellationToken cancellationToken)
        {
            //var test = await responseMessage.Content.ReadAsStringAsync();

            using (var stream = await responseMessage.Content.ReadAsStreamAsync())
            {
                //Todo: Implement cancellationToken support
                using (StreamReader reader = new StreamReader(stream))
                {
                    var result = reader.ReadToEnd();

                    if (Helper.IsHtml(result))
                    {
                        return (T)Activator.CreateInstance(typeof(T), new object[] { result });
                    }
                    else
                    {
                        return JsonConvert.DeserializeObject<T>(result, _serializerSettings);
                    }
                }
            }
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
                {
                    response.Error = errorResponse.Error;
                }
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