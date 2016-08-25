using GTmetrix.Logic;
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
            // Map properties
            var postdata = new List<KeyValuePair<string, string>>();

            postdata.Add(Helper.CreateKvP("url", Url));
            postdata.Add(Helper.CreateKvP("location", Location.ToString()));
            postdata.Add(Helper.CreateKvP("browser", Browser.ToString()));
            postdata.Add(Helper.CreateKvP("x-metrix-adblock", Convert.ToInt32(EnableAdBlock).ToString()));
            postdata.Add(Helper.CreateKvP("x-metrix-video", Convert.ToInt32(GenerateVideo).ToString()));

            if (!string.IsNullOrEmpty(LoginUser))
            {
                postdata.Add(Helper.CreateKvP("login-user", LoginUser));
                postdata.Add(Helper.CreateKvP("login-pass", LoginPass));
            }

            if (!string.IsNullOrEmpty(CookieData))
            {
                postdata.Add(Helper.CreateKvP("x-metrix-cookies", LoginUser));
            }

            if (!string.IsNullOrEmpty(UrlWhitelist))
            {
                postdata.Add(Helper.CreateKvP("x-metrix-whitelist", UrlWhitelist));
            }

            if (!string.IsNullOrEmpty(UrlBlacklist))
            {
                postdata.Add(Helper.CreateKvP("x-metrix-blacklist", UrlBlacklist));
            }

            if (!string.IsNullOrEmpty(ConnectionSpeed))
            {
                postdata.Add(Helper.CreateKvP("x-metrix-throttle", ConnectionSpeed));
            }

            return postdata;
        }
    }
}