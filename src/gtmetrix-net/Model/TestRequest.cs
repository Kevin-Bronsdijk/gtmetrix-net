using System;

namespace GTmetrix.Model
{
    public class TestRequest : TestRequestBase
    {
        public TestRequest(Uri url) : this(url, Locations.Vancouver, Browsers.Chrome)
        {
        }

        public TestRequest(Uri url, Locations location) : this(url, location, Browsers.Chrome)
        {
        }

        public TestRequest(Uri url, Locations location, Browsers browsers)
        {
            Url = url.OriginalString;
            Location = (int)location;
            Browser = (int)browsers;
        }
    }
}