using System;
using System.Threading;
using System.Threading.Tasks;
using GTmetrix.Http;
using GTmetrix.Model;
using System.Net.Http;
using System.Collections.Generic;
using System.IO;
using GTmetrix.Logic;

namespace GTmetrix
{
    public class Client : IDisposable
    {
        private Connection _connection;

        public Client(Connection connection)
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

        public Task<IApiResponse<SubmitTestResult>> SubmitTest(ITestRequest testRequest)
        {
            return SubmitTest(testRequest, default(CancellationToken));
        }

        public Task<IApiResponse<SubmitTestResult>> SubmitTest(ITestRequest testRequest, CancellationToken cancellationToken)
        {
            if (testRequest == null)
            {
                throw new ArgumentNullException(nameof(testRequest));
            }
            if (cancellationToken == null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }

            var message = _connection.Execute<SubmitTestResult>(
                new ApiRequest(testRequest, "0.1/test", HttpMethod.Post),
                cancellationToken);

            return message;
        }

        public Task<IApiResponse<TestResult>> GetTest(string testId)
        {
            return GetTest(testId, default(CancellationToken));
        }

        public Task<IApiResponse<TestResult>> GetTest(string testId, CancellationToken cancellationToken)
        {
            testId.ThrowIfNullOrEmpty(nameof(testId));

            if (cancellationToken == null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }

            var message = _connection.Execute<TestResult>(new ApiRequest(
                new NoInstructionsRequest(), "0.1/test/" + testId, HttpMethod.Get),
                cancellationToken);

            return message;
        }

        public Task<IApiResponse<List<Location>>> Locations()
        {
            return Locations(default(CancellationToken));
        }

        public Task<IApiResponse<List<Location>>> Locations(CancellationToken cancellationToken)
        {
            if (cancellationToken == null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }

            var message = _connection.Execute<List<Location>>
                (
                new ApiRequest(new NoInstructionsRequest(), "0.1/locations", HttpMethod.Get),
                cancellationToken
                );

            return message;
        }

        public Task<IApiResponse<List<Browser>>> Browsers()
        {
            return Browsers(default(CancellationToken));
        }

        public Task<IApiResponse<List<Browser>>> Browsers(CancellationToken cancellationToken)
        {
            if (cancellationToken == null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }

            var message = _connection.Execute<List<Browser>>
                (
                new ApiRequest(new NoInstructionsRequest(), "0.1/browsers", HttpMethod.Get),
                cancellationToken
                );

            return message;
        }

        public Task<IApiResponse<Status>> Status()
        {
            return Status(default(CancellationToken));
        }

        public Task<IApiResponse<Status>> Status(CancellationToken cancellationToken)
        {
            if (cancellationToken == null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }

            var message = _connection.Execute<Status>
                (
                new ApiRequest(new NoInstructionsRequest(), "0.1/status", HttpMethod.Get),
                cancellationToken
                );

            return message;
        }

        public Task<IApiResponse<Byte[]>> DownloadResource(string testId, ResourceTypes resourceType)
        {
            return DownloadResource(testId, resourceType, default(CancellationToken));
        }

        public Task<IApiResponse<Byte[]>> DownloadResource(string testId, ResourceTypes resourceType, 
            CancellationToken cancellationToken)
        {
            testId.ThrowIfNullOrEmpty(nameof(testId));

            if (cancellationToken == null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }

            var message = _connection.Download
                (
                    new ApiRequest(new NoInstructionsRequest(), 
                        $"0.1/test/{testId}/{Helper.GetResourceUriSuffix(resourceType)}", HttpMethod.Get),
                        cancellationToken
                );

            return message;
        }

        ~Client()
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