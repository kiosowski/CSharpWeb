using SIS.HTTP.Common;
using SIS.HTTP.Cookies;
using SIS.HTTP.Cookies.Contracts;
using SIS.HTTP.Extensions;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Contracts;
using SIS.HTTP.Response.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SIS.HTTP.Response
{
    public class HttpResponse : IHttpResponse
    {
        public HttpResponse()
        {

        }
        public HttpResponse(HttpStatusCode statusCode)
        {
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookiesCollection();
            this.Content = new byte[0];
            this.StatusCode = statusCode;
        }
        public HttpStatusCode StatusCode { get; set; }

        public IHttpHeadersCollection Headers { get;}

        public byte[] Content { get; set; }

        public IHttpCookieCollection Cookies { get;  }

        public void AddCookie(HttpCookie cookie)
        {
            this.Cookies.Add(cookie);
        }

        public void AddHeader(HttpHeader header)
        {
            this.Headers.Add(header);
        }

        public byte[] GetBytes()
        {
            return Encoding.UTF8
                .GetBytes(this.ToString())
                .Concat(this.Content)
                .ToArray();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result
                .AppendLine($"{GlobalConstants.HttpOneProtocolFragment} {this.StatusCode.GetResponseLine()}")
                .AppendLine($"{this.Headers}");

            if (this.Cookies.HasCookies())
            {
                result.AppendLine($"Cookie: {this.Cookies}");
            }
            result.AppendLine();
                

            return result.ToString();
        }
    }
}
