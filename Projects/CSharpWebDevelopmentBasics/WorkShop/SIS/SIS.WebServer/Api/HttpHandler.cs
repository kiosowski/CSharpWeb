using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Results;
using SIS.WebServer.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SIS.WebServer.Api
{
    public class HttpHandler : IHttpHandler
    {
        private ServerRoutingTable serverRoutingTable;

        public HttpHandler(ServerRoutingTable routingTable)
        {
            this.serverRoutingTable = routingTable;
        }
        public IHttpResponse Handle(IHttpRequest httpRequest)
        {
            var isResourceRequest = this.IsResourceRequest(httpRequest);
            if (isResourceRequest)
            {
                return this.HandleRequestResponse(httpRequest.Path);
            }

            if (!this.serverRoutingTable.Routes.ContainsKey(httpRequest.RequestMethod)
                || !this.serverRoutingTable.Routes[httpRequest.RequestMethod].ContainsKey(httpRequest.Path))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            return this.serverRoutingTable.Routes[httpRequest.RequestMethod][httpRequest.Path].Invoke(httpRequest);
        }

        private bool IsResourceRequest(IHttpRequest httpRequest)
        {
            var requestPath = httpRequest.Path;
            if (requestPath.Contains('.'))
            {
                var requestPathExtension = requestPath.Substring(requestPath.LastIndexOf('.'));
                var resourceExtension = requestPath.Contains(requestPathExtension);

            }
            return false;
        }
        private IHttpResponse HandleRequestResponse(string path)
        {
            var indexOfStartOfExtension = path.LastIndexOf('.');
            var indexOfStartNameOfResource = path.LastIndexOf('/');
            var requestPathExtension = path.Substring(indexOfStartOfExtension);

            var resourceName = path.Substring(indexOfStartNameOfResource);

            var resourcePath = "../../.."
                                + "/Resources"
                                + $"/{requestPathExtension.Substring(1)}"
                                + resourceName;
            if (!File.Exists(resourcePath))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }
            var fileContent = File.ReadAllBytes(resourcePath);
            return new InlineResourceResult(fileContent, HttpResponseStatusCode.Ok);
        }
    }
}
