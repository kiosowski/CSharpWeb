namespace WebServerHTTPProtocol
{
    using System;
    using System.Net;
    public static class Program
    {
        public static void Main(string[] args)
        {
            string input = Console.ReadLine();
            Console.WriteLine(WebUtility.UrlDecode(input));
        }
    }
}
