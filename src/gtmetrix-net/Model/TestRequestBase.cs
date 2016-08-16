using System;
using System.Collections.Generic;

namespace GTmetrix.Model
{
    public abstract class TestRequestBase : ITestRequest, IRequest
    {
        internal string Url { get; set; }

        public int Location { get; set; }

        public int Browser { get; set; }

        public string LoginUser {get; set;}

        public string LoginPass { get; set; }

        public bool EnableAdBlock { get; set; } = false;

        public string CookieData { get; set; }

        public bool GenerateVideo { get; set; } = false;

        public string UrlWhitelist { get; set; }

        public string UrlBlacklist { get; set; }

        public string ConnectionSpeed { get; set; }

        public List<KeyValuePair<string, string>> GetPostData()
        {
            // Map properties, no reflection
            var postdata = new List<KeyValuePair<string, string>>();

            postdata.Add(CreateKvP("url", Url));
            postdata.Add(CreateKvP("location", Location.ToString()));
            postdata.Add(CreateKvP("browser", Browser.ToString()));
            postdata.Add(CreateKvP("x-metrix-adblock", Convert.ToInt32(EnableAdBlock).ToString()));
            postdata.Add(CreateKvP("x-metrix-video", Convert.ToInt32(GenerateVideo).ToString()));

            if (!string.IsNullOrEmpty(LoginUser))
            {
                postdata.Add(CreateKvP("login-user", LoginUser));
                postdata.Add(CreateKvP("login-pass", LoginPass));
            }

            if (!string.IsNullOrEmpty(CookieData))
            {
                postdata.Add(CreateKvP("x-metrix-cookies", LoginUser));
            }

            if (!string.IsNullOrEmpty(UrlWhitelist))
            {
                postdata.Add(CreateKvP("x-metrix-whitelist", UrlWhitelist));
            }

            if (!string.IsNullOrEmpty(UrlBlacklist))
            {
                postdata.Add(CreateKvP("x-metrix-blacklist", UrlBlacklist));
            }

            if (!string.IsNullOrEmpty(ConnectionSpeed))
            {
                postdata.Add(CreateKvP("x-metrix-throttle", ConnectionSpeed));
            }

            return postdata;
        }

        internal KeyValuePair<string, string> CreateKvP(string key, string value)
        {
            return new KeyValuePair<string, string>(key, value);
        }
    }
}