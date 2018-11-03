using SIS.HTTP.Sessions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Sessions
{
    public class HttpSession : IHttpSession
    {
        public readonly IDictionary<string,object> parameters;
        public HttpSession(string id)
        {
            this.Id = Id;
            this.parameters = new Dictionary<string, object>();
        }
        public string Id { get; }

        public object GetParameter(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException();
            }
            if (this.ContainsParameter(name))
            {
                return null;
            }
            return this.parameters[name];
        }

        public void AddParameter(string name, object parameter)
        {
            if (this.ContainsParameter(name))
            {
                throw new ArgumentException();
            }
            this.parameters[name] = parameter;
        }

        public bool ContainsParameter(string name)
        {
            return this.parameters.ContainsKey(name);
        }

        public void ClearParameteres()
        {
            this.parameters.Clear();
        }
    }
}
