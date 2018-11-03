using SIS.HTTP.Requests;
using SIS.HTTP.Responses;

namespace IRunes.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                var username = request.Session.GetParameter("username");
                this.ViewBag["username"] = username.ToString();
                //this.ViewBag["auth"] = "none";
                return this.View();
            }
            //this.ViewBag["auth"] = "none";
            return this.View();
        }
    }
}