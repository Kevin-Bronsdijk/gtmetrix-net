using GTmetrix5;
using GTmetrix5.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests_net5
{
    public static class HelperFunctions
    {
        public static Client CreateWorkingClient()
        {
            var connection = Connection.Create(Settings.ApiKey, Settings.Email);
            return new Client(connection);
        }

        public static UserSettingsClient CreateWorkingUserSettingsClient()
        {
            var connection = UserSettingsConnection.Create(Settings.Email, Settings.Password);
            return new UserSettingsClient(connection);
        }

        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
