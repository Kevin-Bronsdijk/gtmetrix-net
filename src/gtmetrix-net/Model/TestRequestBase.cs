using System.Collections.Generic;

namespace GTmetrix.Model
{
    public abstract class TestRequestBase : ITestRequest, IRequest
    {
        internal string Url { get; set; }

        public int Location { get; set; }

        public int Browser { get; set; }

        public List<KeyValuePair<string, string>> GetPostData()
        {
            // Map properties, no reflection
            var postdata = new List<KeyValuePair<string, string>>();

            postdata.Add(CreateKvP("url", Url));
            postdata.Add(CreateKvP("location", Location.ToString()));
            postdata.Add(CreateKvP("browser", Browser.ToString()));

            return postdata;
        }

        internal KeyValuePair<string, string> CreateKvP(string key, string value)
        {
            return new KeyValuePair<string, string>(key, value);
        }
    }
}