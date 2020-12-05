using GTmetrix5.Http;
using GTmetrix5.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GTmetrix5.Logic
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
                {"Yslow", "yslow" },
                {"PagespeedFiles", "pagespeed-files"},
                {"Screenshot", "screenshot"},
                {"PdfReport", "report-pdf"},
                {"PdfReportExtended", "report-pdf?full=1"},
                {"Video", "video"},
            };

            string value;
            resourceTypes.TryGetValue(resourceType.ToString(), out value);

            return value;
        }

        public static string GetConnectionSpeed(ConnectionTypes connectionType)
        {
            var resourceTypes = new Dictionary<string, string>
            {
                {"Unthrottled", ""},
                {"Cable", "5000/1000/30"},
                {"DSL", "1500/384/50" },
                {"Mobile3G", "1600/768/200"},
                {"Mobile2G", "240/200/400" },
                {"56K", "50/30/125"}
            };

            string value;
            resourceTypes.TryGetValue(connectionType.ToString(), out value);

            return value;
        }

        internal static byte[] ReadFully(Stream input)
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

        internal static ApiResponse<TResponse> CreateFailedResponse<TResponse>(string message,
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            var response = new ApiResponse<TResponse>()
            {
                Error = message,
                StatusCode = statusCode,
                Success = false
            };

            return response;
        }

        internal static KeyValuePair<string, string> CreateKvP(string key, string value)
        {
            return new KeyValuePair<string, string>(key, value);
        }

        public static class Html
        {
            internal static bool IsHtml(string value)
            {
                // Sufficient for now
                return value.Contains("html");
            }

            internal static string FindInputValue(string source, string find)
            {
                try
                {
                    var pos = source.IndexOf(find);
                    var posVal = source.IndexOf("value", pos);
                    return source.Substring(posVal + 7, 255).Split('"')[0];
                }
                catch (Exception)
                {
                    //Todo: Create parse Exception
                    return string.Empty;
                }
            }

            internal static string FindSelectedOption(string source, string find)
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
                    //Todo: Create parse Exception
                    return string.Empty;
                }
            }

            internal static List<string> FindTextAreaValues(string source, string find)
            {
                List<string> list = new List<string>();

                try
                {
                    var pos = source.IndexOf(find);
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
                    //Todo: Create parse Exception
                }

                return list;
            }
        }
    }
}
