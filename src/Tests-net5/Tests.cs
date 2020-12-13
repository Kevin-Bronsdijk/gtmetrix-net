using GTmetrix5;
using GTmetrix5.Http;
using GTmetrix5.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests_net5
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void ConnectionCreate_EmptyKeyError_IsTrue()
        {
            try
            {
                Connection.Create("", "username");
                Assert.IsTrue(false);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message == "Argument must not be the empty string. (Parameter 'apiKey')");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void ConnectionCreate_NullKeyError_IsTrue()
        {
            try
            {
                Connection.Create(null, "username");
                Assert.IsTrue(false);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message == "Value cannot be null. (Parameter 'apiKey')");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void ConnectionCreate_EmptySecretError_IsTrue()
        {
            try
            {
                Connection.Create("key", "");
                Assert.IsTrue(false);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message == "Argument must not be the empty string. (Parameter 'username')");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void ConnectionCreate_NullSecretError_IsTrue()
        {
            try
            {
                Connection.Create("key", null);
                Assert.IsTrue(false);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message == "Value cannot be null. (Parameter 'username')");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void Client_NullConnectionError_IsTrue()
        {
            try
            {
                var client = new Client(null);
                Assert.IsTrue(false);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message == "Value cannot be null. (Parameter 'connection')");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void Client_SubmitTestRequestCannotBeNull_IsTrue()
        {
            try
            {
                var connection = Connection.Create("key", "secret");
                var client = new Client(connection);

                client.SubmitTest(null);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message == "Value cannot be null. (Parameter 'testRequest')");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void Client_GetTestRequestTestIdCannotBeNull_IsTrue()
        {
            try
            {
                var connection = Connection.Create("key", "secret");
                var client = new Client(connection);

                client.GetTest(null);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message == "Value cannot be null. (Parameter 'testId')");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void Client_GetTestRequestTestIdCannotBeEmpty_IsTrue()
        {
            try
            {
                var connection = Connection.Create("key", "secret");
                var client = new Client(connection);

                client.GetTest(string.Empty);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message == "Argument must not be the empty string. (Parameter 'testId')");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void Client_DownloadResourceIdCannotBeNull_IsTrue()
        {
            try
            {
                var connection = Connection.Create("key", "secret");
                var client = new Client(connection);

                client.DownloadResource(null, ResourceTypes.PageSpeed);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message == "Value cannot be null. (Parameter 'testId')");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void Client_DownloadResourceIdCannotBeEmpty_IsTrue()
        {
            try
            {
                var connection = Connection.Create("key", "secret");
                var client = new Client(connection);

                client.DownloadResource(string.Empty, ResourceTypes.PageSpeed);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message == "Argument must not be the empty string. (Parameter 'testId')");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void Client_NoErrors_IsTrue()
        {
            try
            {
                var connection = Connection.Create("key", "secret");
                var client = new Client(connection);

                Assert.IsTrue(client != null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void ConnectionCreate_Dispose_IsTrue()
        {
            var connection = Connection.Create("key", "secret");

            try
            {
                connection.Dispose();

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void Client_MustProvideAConnection_IsTrue()
        {
            try
            {
                var client = new Client(null);

                Assert.IsTrue(false, "No exception");
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message == "Value cannot be null. (Parameter 'connection')");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void Client_Dispose_IsTrue()
        {
            var connection = Connection.Create("key", "secret");
            var client = new Client(connection);

            try
            {
                client.Dispose();

                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.IsTrue(false, "Exception");
            }
        }

        [TestMethod]
        public void Helper_GetUriBasedOnResourceType_IsTrue()
        {
            var suffix = GTmetrix5.Logic.Helper.GetResourceUriSuffix(ResourceTypes.PageSpeed);

            Assert.IsTrue(suffix == "pagespeed");
        }

        [TestMethod]
        public void Helper_GetConnectionSpeedBasedOnConnectionType_IsTrue()
        {
            var value = GTmetrix5.Logic.Helper.GetConnectionSpeed(ConnectionTypes.Unthrottled);

            Assert.IsTrue(value == "");
        }
    }
}
