using SIS.HTTP.Cookies;
using SIS.HTTP.Cookies.Contracts;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Contracts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SIS.HTTP.Response.Contracts
{
    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; set; }
        IHttpHeadersCollection Headers { get; }

        IHttpCookieCollection Cookies { get; }
        byte[] Content { get; set; }

        void AddHeader(HttpHeader header);
        void AddCookie(HttpCookie cookie);
        byte[] GetBytes();
    }
}
