using System;

namespace GTmetrix.Model
{
    public class TestRequest : TestRequestBase
    {
        public TestRequest(Uri url)
        {
           Url = url.OriginalString;
        }
    }
}