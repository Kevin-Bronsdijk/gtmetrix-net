using GTmetrix5.Http;
using GTmetrix5.Logic;
using GTmetrix5.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GTmetrix5
{
    public class Client : IDisposable
    {
        private Connection _connection;
        private const int DefaultRetryInterval = 3;
        private const int MaxRetryCount = 10;

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

        public async Task<IApiResponse<TestResult>> SubmitTestAsync(ITestRequest testRequest)
        {
            return await SubmitTestAsync(testRequest, default(CancellationToken));
        }

        public async Task<IApiResponse<TestResult>> SubmitTestAsync(ITestRequest testRequest, int retryInterval)
        {
            return await SubmitTestAsync(testRequest, retryInterval, default(CancellationToken));
        }

        public async Task<IApiResponse<TestResult>> SubmitTestAsync(ITestRequest testRequest, CancellationToken cancellationToken)
        {
            return await SubmitTestAsync(testRequest, DefaultRetryInterval, cancellationToken);
        }

        public async Task<IApiResponse<TestResult>> SubmitTestAsync(ITestRequest testRequest, int retryInterval,
            CancellationToken cancellationToken)
        {
            if (testRequest == null)
            {
                throw new ArgumentNullException(nameof(testRequest));
            }
            if (cancellationToken == null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }

            ValiadateRetryInterval(retryInterval);

            var submitTestResult = await _connection.Execute<SubmitTestResult>(
                new ApiRequest(testRequest, "0.1/test", HttpMethod.Post),
                cancellationToken).ConfigureAwait(false);

            if (submitTestResult.Success)
            {
                // Always wait first
                await Task.Delay(TimeToWait(retryInterval), cancellationToken);

                var testResult = await GetTestAsync(submitTestResult.Body.TestId, retryInterval,
                    cancellationToken).ConfigureAwait(false);

                return testResult;
            }
            else
            {
                return Helper.CreateFailedResponse<TestResult>(string.Empty, submitTestResult.StatusCode);
            }
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

        public async Task<IApiResponse<TestResult>> GetTestAsync(string testId)
        {
            return await GetTestAsync(testId, default(CancellationToken));
        }

        public async Task<IApiResponse<TestResult>> GetTestAsync(string testId, int retryInterval)
        {
            return await GetTestAsync(testId, retryInterval, default(CancellationToken));
        }

        public async Task<IApiResponse<TestResult>> GetTestAsync(string testId, CancellationToken cancellationToken)
        {
            return await GetTestAsync(testId, DefaultRetryInterval, cancellationToken);
        }

        public async Task<IApiResponse<TestResult>> GetTestAsync(string testId, int retryInterval, CancellationToken cancellationToken)
        {
            int fetchCounter = 0;
            IApiResponse<TestResult> result;

            ValiadateRetryInterval(retryInterval);

            do
            {
                if (fetchCounter > MaxRetryCount)
                {
                    result = Helper.CreateFailedResponse<TestResult>("Maximum retry count exceeded");
                    break;
                }

                result = await GetTest(testId, cancellationToken).ConfigureAwait(false);
                fetchCounter++;

                //Todo: Exception
                if ((result.Success && result.Body.State == ResultStates.Completed) || !result.Success)
                {
                    break;
                }

                await Task.Delay(TimeToWait(retryInterval), cancellationToken);
            }
            while (!cancellationToken.IsCancellationRequested);

            return result;
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

        public Task<IApiResponse<byte[]>> DownloadResource(string testId, ResourceTypes resourceType)
        {
            return DownloadResource(testId, resourceType, default(CancellationToken));
        }

        public Task<IApiResponse<byte[]>> DownloadResource(string testId, ResourceTypes resourceType,
            CancellationToken cancellationToken)
        {
            testId.ThrowIfNullOrEmpty(nameof(testId));

            if (cancellationToken == null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }

            //Todo: Exception
            var message = _connection.Download
                (
                    new ApiRequest(new NoInstructionsRequest(),
                        $"0.1/test/{testId}/{Helper.GetResourceUriSuffix(resourceType)}", HttpMethod.Get),
                        cancellationToken
                );

            return message;
        }

        private TimeSpan TimeToWait(int retryInterval)
        {
            return TimeSpan.FromSeconds(retryInterval);
        }

        private void ValiadateRetryInterval(int retryInterval)
        {
            if (retryInterval < 2 && retryInterval > 60)
            {
                throw new ArgumentException("RetryInterval must be 2 and 60 seconds");
            }
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
