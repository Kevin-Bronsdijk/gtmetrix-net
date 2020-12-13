using GTmetrix5;
using GTmetrix5.Http;
using GTmetrix5.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Tests_net5
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

                if (resultCheck.Body.State == ResultStates.Completed)
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

        [TestMethod]
        public void Client_CheckTestPollStatusResults_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.SubmitTest(new TestRequest(
                new Uri(TestData.TestWebsite),
                Locations.London,
                Browsers.Chrome));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(new TimeSpan(0, 0, 15));

                var responseCheck = client.GetTest(result.Body.TestId);
                var resultCheck = responseCheck.Result;

                if (resultCheck.Body.State == ResultStates.Completed)
                {
                    Assert.IsTrue(resultCheck.StatusCode == HttpStatusCode.OK);
                    Assert.IsTrue(resultCheck.Body.Results.PageLoadTime != 0);

                    // Check resouces
                    var responseResource = client.DownloadResource(result.Body.TestId, ResourceTypes.PageSpeed);
                    var resultResource = responseResource.Result;

                    Assert.IsTrue(resultResource.StatusCode == HttpStatusCode.OK);
                    Assert.IsTrue(resultResource.Body != null);

                    var pageSpeedJson = System.Text.Encoding.UTF8.GetString(resultResource.Body);

                    Assert.IsTrue(pageSpeedJson != string.Empty);
                    Assert.IsTrue(pageSpeedJson.Contains("pageSpeed"));

                    break;
                }
            }
        }

        [TestMethod]
        public void Client_GetTestAsyncResults_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.SubmitTest(new TestRequest(
                new Uri(TestData.TestWebsite),
                Locations.London,
                Browsers.Chrome));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);

            var responseCheck = client.GetTestAsync(result.Body.TestId);
            var resultCheck = responseCheck.Result;

            Assert.IsTrue(resultCheck.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(resultCheck.Body.Results.PageLoadTime != 0);
        }

        [TestMethod]
        public void Client_SubmitTestAsync_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.SubmitTestAsync(new TestRequest(
                new Uri(TestData.TestWebsite),
                Locations.London,
                Browsers.Chrome));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Body.Results.PageLoadTime != 0);
        }

        [TestMethod]
        [Ignore]
        public void Client_DownloadAllResouces_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.SubmitTest(new TestRequest(
                new Uri(TestData.TestWebsite),
                Locations.London,
                Browsers.Chrome)
            {
                GenerateVideo = true
            });

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);

            var responseCheck = client.GetTestAsync(result.Body.TestId);
            var resultCheck = responseCheck.Result;

            Assert.IsTrue(resultCheck.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(resultCheck.Body.Results.PageLoadTime != 0);
            Assert.IsTrue(resultCheck.Body.Resources.VideoUrl != null);

            var resourceTypes = HelperFunctions.GetValues<ResourceTypes>();

            foreach (var resourceType in resourceTypes)
            {
                var responseResource = client.DownloadResource(result.Body.TestId, resourceType);
                File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "\\" + resourceType + "." + resourceType,
                    responseResource.Result.Body);
            }
        }
    }
}
