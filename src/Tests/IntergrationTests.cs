using GTmetrix;
using GTmetrix.Http;
using GTmetrix.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Threading;

namespace Tests
{
    [TestClass]
    [Ignore]
    public class IntergrationTests
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void Client_Unauthorized_IsTrue()
        {
            var connection = Connection.Create("key", "secret");
            var client = new Client(connection);

            var response = client.Locations();

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        public void Client_Authorized_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.Locations();

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public void Client_GetLocations_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.Locations();

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Body.Count != 0);

            foreach (var item in result.Body)
            {
                Assert.IsTrue(item.Id != string.Empty);
                Assert.IsTrue(item.Name != string.Empty);
            }
        }

        [TestMethod]
        public void Client_GetBrowsers_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.Browsers();

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Body.Count != 0);

            foreach (var item in result.Body)
            {
                Assert.IsTrue(item.Id != string.Empty);
                Assert.IsTrue(item.Name != string.Empty);
            }
        }

        [TestMethod]
        public void Client_GetStatus_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.Status();

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Body.Credits != -100);
            Assert.IsTrue(result.Body.Topup != -100);
        }

        [TestMethod]
        public void Client_Test_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.SubmitTest(new TestRequest(new Uri(TestData.TestWebsite)));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Body.PollStateUrl != string.Empty);
            Assert.IsTrue(result.Body.TestId != string.Empty);
        }

        [TestMethod]
        public void Client_CheckTestNotFound_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var response = client.GetTest("12345678");
            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void Client_CheckTestSimple_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.SubmitTest(new TestRequest(new Uri(TestData.TestWebsite)));
            var result = response.Result;

            var responseCheck = client.GetTest(result.Body.TestId);
            var resultCheck = responseCheck.Result;

            Assert.IsTrue(resultCheck.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(resultCheck.Body.State != string.Empty);
        }

        [TestMethod]
        public void Client_CheckTestPollStatus_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.SubmitTest(new TestRequest(new Uri(TestData.TestWebsite)));
            var result = response.Result;

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(new TimeSpan(0, 0, 15));

                var responseCheck = client.GetTest(result.Body.TestId);
                var resultCheck = responseCheck.Result;

                if (resultCheck.Body.State == "completed")
                {
                    Assert.IsTrue(resultCheck.StatusCode == HttpStatusCode.OK);
                    break;
                }
            }
        }

        [TestMethod]
        public void Client_InvalidTestServerRegionId_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var testrequest = new TestRequest(new Uri(TestData.TestWebsite));
            testrequest.Location = TestData.InvalidTestServerRegionId;

            var response = client.SubmitTest(testrequest);
            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void Client_InvalidbrowserId_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var testrequest = new TestRequest(new Uri(TestData.TestWebsite), Locations.London);
            testrequest.Browser = TestData.InvalidbrowserId;

            var response = client.SubmitTest(testrequest);
            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void Client_LondonChrome_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var testrequest = new TestRequest(new Uri(TestData.TestWebsite), 
                Locations.London, Browsers.Chrome);

            var response = client.SubmitTest(testrequest);
            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Body.PollStateUrl != string.Empty);
            Assert.IsTrue(result.Body.TestId != string.Empty);
        }
    }
}