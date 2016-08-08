using GTmetrix.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace GTmetrix.Logic
{
    public static class Helper
    {
        public static void ThrowIfNullOrEmpty(this string value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
            if (value == string.Empty)
            {
                throw new ArgumentException("Argument must not be the empty string.", name);
            }
        }

        public static string GetResourceUriSuffix(ResourceTypes resourceType)
        {
            var resourceTypes = new Dictionary<string, string>
            {
                {"Har", "har"},
                {"PageSpeed", "pagespeed"},
                {"Yslow", "yslow" }
            };

            string abbreviation;
            resourceTypes.TryGetValue(resourceType.ToString(), out abbreviation);

            return abbreviation;
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}