using System;
using System.Collections.Generic;

namespace GTmetrix.Model
{
    public class UserSettings
    {
        public UserSettings(string value)
        {
            Parse(value);
        }

        private void Parse(string value)
        {
            //Simple parsing, don't want to include a new library

            FirstName = FindInputValue(value, "id=\"us-first_name\"");
            LastName = FindInputValue(value, "id=\"us-last_name\"");
            Email = FindInputValue(value, "id=\"us-email\"");
            Alerts = FindInputValue(value, "id=\"us-prefs-alerts_disabled\"").Equals("1");

            Timezone = FindSelectedOption(value, "id=\"us-prefs-timezone\"");
            Region = FindSelectedOption(value, "id=\"us-prefs-region\"");
            DefaultBrowser = FindSelectedOption(value, "id=\"us-prefs-browser\"");

            Cdn = FindCdnInput(value);
        }

        private string FindInputValue(string source, string find)
        {
            try
            {
                var pos = source.IndexOf(find);
                var posVal = source.IndexOf("value", pos);
                return source.Substring(posVal + 7, 255).Split('"')[0]; ;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private string FindSelectedOption(string source, string find)
        {
            //Todo: No selection
            try
            {
                var pos = source.IndexOf(find);
                var posVal = source.IndexOf("selected", pos);
                return source.Substring(posVal + 9, 255).Split('>')[0].Split('<')[0];
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private List<string> FindCdnInput(string source)
        {
            List<string> list = new List<string>();

            try
            {
                var pos = source.IndexOf("id=\"us-prefs-cdn\"");
                var posVal = source.IndexOf("value", pos);
                var items = source.Substring(pos, 255).Split('>')[1].Split('<')[0].Split('\n');

                foreach (var item in items)
                {
                    if (item != string.Empty)
                    {
                        list.Add(item);
                    }
                }
            }
            catch (Exception)
            {
            }

            return list;
        }

        public string FirstName { get; internal set; }

        public string LastName { get; internal set; }

        public string Email { get; internal set; }
        
        public bool Alerts { get; internal set; }

        public string Timezone { get; internal set; }

        public string Region { get; internal set; }

        public string DefaultBrowser { get; internal set; }

        public List<string> Cdn { get; internal set; } = new List<string>();
    }
}
