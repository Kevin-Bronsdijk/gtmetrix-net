using GTmetrix.Model;
using System;
using System.Threading.Tasks;
using Tests;

namespace WebTest.TestLogic
{
    public class Tests
    {
        public static bool CanCreateClient()
        {
            try
            {
                var client = HelperFunctions.CreateWorkingClient();
                return (client != null);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CanGetLocations()
        {
            try
            {
                var client = HelperFunctions.CreateWorkingClient();
                var locations = client.Locations();

                return locations.Result.Success;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async static Task<bool> CanSubmitTestAsync()
        {
            try
            {
                var client = HelperFunctions.CreateWorkingClient();

                var response = client.SubmitTestAsync(
                    new TestRequest(
                        new Uri("http://devslice.net"), Locations.London, Browsers.Chrome)
                        );

                var response2 = client.SubmitTestAsync(
                    new TestRequest(
                        new Uri("https://azure.microsoft.com/en-us/"), Locations.London, Browsers.Chrome)
                        );

                await Task.WhenAll(response, response2);

                if (response.Result.Success && response2.Result.Success)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
