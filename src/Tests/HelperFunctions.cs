using GTmetrix;
using GTmetrix.Http;

namespace Tests
{
    public static class HelperFunctions
    {
        public static Client CreateWorkingClient()
        {
            var connection = Connection.Create(Settings.ApiKey, Settings.Username);
            return new Client(connection);
        }
    }
}