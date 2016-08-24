﻿using GTmetrix.Http;
using GTmetrix.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

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

        internal static bool IsHtml(string value)
        {
            // Sufficient for now
            return value.Contains("html");
        }
    }
}