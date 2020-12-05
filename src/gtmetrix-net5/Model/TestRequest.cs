using GTmetrix5.Logic;
using System;

namespace GTmetrix5.Model
{
    public class TestRequest : TestRequestBase
    {
        public TestRequest(Uri url) : this(url, Locations.Vancouver, Browsers.Chrome)
        {
        }

        public TestRequest(Uri url, Locations location) :
            this(url, location, Browsers.Chrome)
        {
        }

        public TestRequest(Uri url, Locations location, Browsers browser) :
            this(url, location, browser, ConnectionTypes.Unthrottled)
        {
        }

        public TestRequest(Uri url, Locations location, Browsers browser, ConnectionTypes connectionType)
        {
            Url = url.OriginalString;
            Location = (int)location;
            Browser = (int)browser;
            ConnectionSpeed = Helper.GetConnectionSpeed(connectionType);
        }
    }
}
