using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Tests
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
    }
}