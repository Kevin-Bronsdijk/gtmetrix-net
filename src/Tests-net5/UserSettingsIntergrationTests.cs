using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Tests_net5
{
    [TestClass]
    [Ignore]
    public class UserSettingsIntergrationTests
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void UserSettingsClient_GetSettings_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingUserSettingsClient();

            var response = client.GetSettings();

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Body.Email == Settings.Email);
        }

        [TestMethod]
        public void UserSettingsClient_UpdateSettings_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingUserSettingsClient();
            var response = client.GetSettings();
            var userSettings = response.Result.Body;

            string newFirstName = "Kevin";
            if (userSettings.FirstName == "Kevin")
            {
                userSettings.FirstName = "Devslice";
                newFirstName = "Devslice";
            }
            else
            {
                userSettings.FirstName = "Kevin";
            }

            var responseAfterUpdate = client.UpdateSettings(userSettings);
            var resultAfterUpdate = responseAfterUpdate.Result;

            Assert.IsTrue(resultAfterUpdate.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(resultAfterUpdate.Body.FirstName == newFirstName);
        }
    }
}
