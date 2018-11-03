using System.Net;

namespace SIS.HTTP.Extensions
{
    public static class HttpResponseStatusExtentions
    {
       public static string GetResponseLine(this HttpStatusCode statusCode) 
            => $"{(int) statusCode} {statusCode}";

    }
}
