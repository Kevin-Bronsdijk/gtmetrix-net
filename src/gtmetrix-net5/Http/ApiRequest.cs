﻿using GTmetrix5.Model;
using System.Net.Http;

namespace GTmetrix5.Http
{
    internal class ApiRequest : IApiRequest
    {
        public ApiRequest(IRequest body, string uri, HttpMethod method)
        {
            Uri = uri;
            Method = method;
            Body = body;
        }

        public ApiRequest(IRequest body, string uri, HttpMethod method, string referer) : this(body, uri, method)
        {
            Referer = referer;
        }

        public string Uri { get; set; }
        public HttpMethod Method { get; set; }
        public IRequest Body { get; set; }
        public string Referer { get; set; }
    }
}
