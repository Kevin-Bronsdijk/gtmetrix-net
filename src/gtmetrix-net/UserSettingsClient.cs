using System;
using System.Threading;
using System.Threading.Tasks;
using GTmetrix.Http;
using GTmetrix.Model;
using System.Net.Http;

namespace GTmetrix
{
    /// <summary>
    /// This is not part of the official API and therefore exposed separately.
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