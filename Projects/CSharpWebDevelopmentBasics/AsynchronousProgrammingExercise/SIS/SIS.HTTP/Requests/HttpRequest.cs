using System;
using System.Collections.Generic;
using System.Linq;
using SIS.HTTP.Common;
using SIS.HTTP.Cookies;
using SIS.HTTP.Cookies.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Exceptions;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Contracts;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Sessions.Contracts;

namespace SIS.HTTP.Requests
{
    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookiesCollection();
            this.ParseRequest(requestString);
        }

        private void ParseRequest(string requestString)
        {
            string[] splitRequestContent = requestString.Split(Environment.NewLine);

            string[] requestLine = splitRequestContent[0].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (!this.IsValidRequestLine(requestLine))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLine);
            this.ParseRequestUrl(requestLine);
            this.ParseRequestPath();

            this.ParseHeaders(splitRequestContent.Skip(1).ToArray());
            this.ParseCookies();
            var requestHasBody = false;
            if (splitRequestContent.Length > 1)
            {
                requestHasBody = true;
            }
            this.ParseRequestParameters(splitRequestContent[splitRequestContent.Length - 1], requestHasBody);
        }

        private void ParseCookies()
        {
            if (!this.Headers.ContainsHeader("Cookie"))
            {
                return;
            }

            var cookieRaw = this.Headers.GetHeader("Cookie").Value;
            var cookies = cookieRaw.Split("; ", StringSplitOptions.RemoveEmptyEntries);
            foreach (var cookie in cookies)
            {
                var cookieKeyValuePair = cookie.Split("=",2);
                if (cookieKeyValuePair.Length != 2)
                {
                    throw new BadRequestException();
                }
                var cookieName = cookieKeyValuePair[0];
                var cookieValue = cookieKeyValuePair[1];
                this.Cookies.Add(new HttpCookie(cookieName, cookieValue));
            }
        }

        private void ParseRequestParameters(string bodyParameters, bool requestHasBody)
        {
            this.ParseQueryParameters(this.Url);
            if (requestHasBody)
            {
                this.ParseFormDataParameters(bodyParameters);
            }
        }

        private void ParseFormDataParameters(string bodyParameters)
        {
            var formDataKeyValuePairs = bodyParameters.Split('&', StringSplitOptions.RemoveEmptyEntries);

            ExtractRequestParameters(formDataKeyValuePairs,this.FormData);
        }

        private void ExtractRequestParameters(string[] parameterKeyValuePairs,Dictionary<string,object> parametersCollection)
        {
            foreach (var parameterKeyValuePair in parameterKeyValuePairs)
            {
                var keyValuePair = parameterKeyValuePair.Split('=', StringSplitOptions.RemoveEmptyEntries);
                if (keyValuePair.Length != 2)
                {
                    throw new BadRequestException();
                }
                var parameterKey = keyValuePair[0];
                var parameterValue = keyValuePair[1];

                parametersCollection[parameterKey] = parameterValue;
            }
        }

        private void ParseQueryParameters(string url)
        {
            var queryString = this.Url.Split("?");
            if (queryString.Length<=1)
            {
                return;
            }
            var queryParameters = queryString.Last().Split("&");

            if (IsValidRequestQueryString(queryString.Last(), queryParameters))
            {
                throw new BadRequestException();
            }
            ExtractRequestParameters(queryParameters, this.QueryData);
        }

        private bool IsValidRequestQueryString(string queryString, string[] queryParameters)
        {
            if (!string.IsNullOrEmpty(queryString) && queryString.Length > 0)
            {
                return true;
            }
            return false;
        }

        private void ParseHeaders(string[] requestHeaders)
        {
            if (!requestHeaders.Any())
            {
                throw new BadRequestException();
            }
            foreach (var requestHeader in requestHeaders)
            {
                if (string.IsNullOrEmpty(requestHeader))
                {
                    return;
                }
                var splitRequestHeader = requestHeader.Split(": ", StringSplitOptions.RemoveEmptyEntries);

                var requestHeaderKey = splitRequestHeader[0];
                var requestHeaderValue = splitRequestHeader[1];

                this.Headers.Add(new HttpHeader(requestHeaderKey, requestHeaderValue));
            }

        }

        private void ParseRequestPath()
        {
            var path = this.Url?.Split('?')
                               .FirstOrDefault();
            if (string.IsNullOrEmpty(path))
            {
                throw new BadRequestException();
            }
            this.Path = path;
        }

        private void ParseRequestUrl(string[] requestLine)
        {
            if (string.IsNullOrEmpty(requestLine[1]))
            {
                throw new BadRequestException();
            }
            this.Url = requestLine[1];
        }

        private void ParseRequestMethod(string[] requestLine)
        {
            if (!requestLine.Any())
            {
                throw new BadRequestException();
            }
            var parseResult = Enum.TryParse<HttpRequestMethod>(requestLine[0],out var parseRequestMethod);

            if (!parseResult)
            {
                throw new BadRequestException();
            }
            this.RequestMethod = parseRequestMethod;

        }

        private bool IsValidRequestLine(string[] requestLine)
        {
            if (!requestLine.Any())
            {
                throw new BadRequestException();
            }
            if (requestLine.Length == 3 && requestLine[2] == GlobalConstants.HttpOneProtocolFragment)
            {
                return true;
            }
            return false;
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeadersCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public IHttpCookieCollection Cookies { get; }
        public IHttpSession Session { get; set; }
    }
}
