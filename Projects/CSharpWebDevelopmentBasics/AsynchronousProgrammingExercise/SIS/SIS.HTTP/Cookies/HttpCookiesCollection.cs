using SIS.HTTP.Cookies.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIS.HTTP.Cookies
{
    public class HttpCookiesCollection : IHttpCookieCollection
    {
        private readonly IDictionary<string, HttpCookie> cookies;

        public HttpCookiesCollection()
        {
            this.cookies = new Dictionary<string, HttpCookie>();
        }
        public void Add(HttpCookie cookie)
        {
            if (cookie == null || this.ContainsCookie(cookie.Key))
            {
                throw new ArgumentNullException();
            }
            this.cookies[cookie.Key] = cookie;
        }

        public HttpCookie GetCookie(string key)
        {
            if (!this.ContainsCookie(key))
            {
                return null;
            }
            return this.cookies[key];
        }

        public bool ContainsCookie(string key)
        {
            return this.cookies.ContainsKey(key);
        }
       
        public bool HasCookies() => this.cookies.Any();

        public override string ToString()
        {
            return string.Join("; ", this.cookies.Values);
        }

    }
}
