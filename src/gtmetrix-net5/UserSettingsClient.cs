using GTmetrix5.Http;
using GTmetrix5.Logic;
using GTmetrix5.Model;
using GTmetrix5.Model.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GTmetrix5
{
    /// <summary>
    /// This isn't part of the official API and therefore exposed separately.
    /// </summary>
    public class UserSettingsClient : IDisposable
    {
        private UserSettingsConnection _connection;

        public UserSettingsClient(UserSettingsConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            _connection = connection;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task<IApiResponse<UserSettings>> GetSettings()
        {
            return GetSettings(default(CancellationToken));
        }

        public Task<IApiResponse<UserSettings>> GetSettings(CancellationToken cancellationToken)
        {
            if (cancellationToken == null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }

            var message = _connection.Execute<UserSettings>
                (
                new ApiRequest(new NoInstructionsRequest(), "dashboard/user_settings", HttpMethod.Get),
                cancellationToken
                );

            return message;
        }

        public Task<IApiResponse<UserSettings>> UpdateSettings(UserSettings userSettings)
        {
            return UpdateSettings(userSettings, default(CancellationToken));
        }

        public Task<IApiResponse<UserSettings>> UpdateSettings(UserSettings userSettings, CancellationToken cancellationToken)
        {
            if (cancellationToken == null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }

            IApiResponse<UserSettings> result;

            var message = _connection.Execute<UserSettingsRequestResult>
                (
                new ApiRequest(userSettings, "dashboard/user_settings", HttpMethod.Post, "dashboard/user_settings"),
                cancellationToken
                );

            if (message.Result.Body != null && message.Result.Body.Success)
            {
                return GetSettings();
            }
            else
            {
                result = Helper.CreateFailedResponse<UserSettings>("Unable to save settings", HttpStatusCode.BadRequest);
                return Task.FromResult(result);
            }
        }

        ~UserSettingsClient()
        {
            Dispose(false);
        }

        internal virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_connection != null)
                {
                    _connection.Dispose();
                    _connection = null;
                }
            }
        }
    }
}
