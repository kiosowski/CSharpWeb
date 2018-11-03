using CakesWebApp.Services;
using IRunes.Data;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace IRunes.Controllers
{
    public abstract class BaseController
    {

        private const string RootDirectoryRelativePath = "../../../";

        private const string ViewsFolderName = "Views";

        private const string DirectorySeparator = "/";

        private const string LayoutFileName = "_Layout";

        private const string RenderBodyConstant = "@RenderBody()";

        private string GetCurrentControllerName() => this.GetType().Name.Replace("Controller", string.Empty);

        private readonly UserCookieService cookieService;

        protected IRunesDbContext Context { get; set; }

        protected Dictionary<string, string> ViewBag { get; set; }

        public BaseController()
        {
            this.Context = new IRunesDbContext();
            this.cookieService = new UserCookieService();
            this.ViewBag = new Dictionary<string, string>();
        }

        public bool IsAuthenticated(IHttpRequest request)
        {
            if (request.Session.ContainsParameter("username"))
            {
                this.ViewBag["isNotAuthenticated"] = "d-none";
                this.ViewBag["isAuthenticated"] = "";
                return true;
            }
            this.ViewBag["isNotAuthenticated"] = "";
            this.ViewBag["isAuthenticated"] = "d-none";
            return false;

        }

        public void SignInUser(string username, IHttpRequest request, IHttpResponse response)
        {
            request.Session.AddParameter("username", username);

            var userCookieValue = this.cookieService.GetUserCookie(username);

            response.Cookies.Add(new HttpCookie("Irunes_auth", userCookieValue));
        }

        public void SignOutUser(IHttpRequest request)
        {
            request.Session.ClearParameters();

            var cookie = request.Cookies.GetCookie("Irunes_auth");

            cookie.Delete();
        }

        protected IHttpResponse View([CallerMemberName] string viewName = "")
        {
            var layoutView = RootDirectoryRelativePath
                 + ViewsFolderName
                 + DirectorySeparator
                 + LayoutFileName
                 + ".html";

            string filePath = RootDirectoryRelativePath
                 + ViewsFolderName
                 + DirectorySeparator
                 + this.GetCurrentControllerName()
                 + DirectorySeparator
                 + viewName
                 + ".html";

            if (!File.Exists(filePath))
            {
                return new BadRequestResult($"View {viewName} not found.", HttpResponseStatusCode.NotFound);
            }

            var viewContent = BuildViewContent(filePath);
            
            var viewLayout = BuildViewContent(layoutView);

            var view = viewLayout.Replace(RenderBodyConstant, viewContent);

            var response = new HtmlResult(view, HttpResponseStatusCode.Ok);

            return response;
        }

        private string BuildViewContent(string filePath)
        {
            var viewContent = File.ReadAllText(filePath);

            foreach (var viewbagKey in ViewBag.Keys)
            {
                var dynamicDataPlaceHolder = $"{{{{{viewbagKey}}}}}";

                if (viewContent.Contains(dynamicDataPlaceHolder))
                {
                    viewContent = viewContent.Replace(dynamicDataPlaceHolder, this.ViewBag[viewbagKey]);
                }
            }

            return viewContent;
        }
    }
}