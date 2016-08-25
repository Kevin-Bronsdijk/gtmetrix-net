using System.Collections.Generic;
using GTmetrix.Logic;

namespace GTmetrix.Model
{
    /// <summary>
    /// This isn't part of the official API and therefore exposed separately.
    /// </summary>
    public class UserSettings : ITestRequest, IRequest
    {
        public UserSettings(string html)
        {
            Parse(html);
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        
        public string Timezone { get; internal set; }

        public string Region { get; internal set; }

        public string DefaultBrowser { get; internal set; }

        public List<string> Cdn { get; internal set; } = new List<string>();

        private void Parse(string html)
        {
            // Simple parsing, don't want to include a new library
            FirstName = Helper.Html.FindInputValue(html, "id=\"us-first_name\"");
            LastName = Helper.Html.FindInputValue(html, "id=\"us-last_name\"");
            Email = Helper.Html.FindInputValue(html, "id=\"us-email\"");

            Timezone = Helper.Html.FindSelectedOption(html, "id=\"us-prefs-timezone\"");
            Region = Helper.Html.FindSelectedOption(html, "id=\"us-prefs-region\"");
            DefaultBrowser = Helper.Html.FindSelectedOption(html, "id=\"us-prefs-browser\"");

            Cdn = Helper.Html.FindTextAreaValues(html, "id=\"us-prefs-cdn\"");
        }

        public List<KeyValuePair<string, string>> GetPostData()
        {
            // Map properties
            var postdata = new List<KeyValuePair<string, string>>();

            postdata.Add(Helper.CreateKvP("first_name", FirstName));
            postdata.Add(Helper.CreateKvP("last_name", LastName));
            postdata.Add(Helper.CreateKvP("email", Email));
            postdata.Add(Helper.CreateKvP("prefs:cdn", FormatCdnNewLine()));

            return postdata;
        }

        private string FormatCdnNewLine()
        {
            return string.Join("\n", Cdn);
        }
    }
}
