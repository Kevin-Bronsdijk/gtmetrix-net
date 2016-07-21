using GTmetrix.Model;
using System;
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
    }
}