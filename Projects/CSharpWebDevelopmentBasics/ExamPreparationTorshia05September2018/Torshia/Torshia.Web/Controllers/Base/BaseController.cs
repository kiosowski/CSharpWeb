using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using SIS.Framework.ActionResults;
using SIS.Framework.Controllers;
using Torshia.Data;
using Torshia.Models.Enums;

namespace Torshia.Web.Controllers.Base
{
    public class BaseController : Controller
    {
        protected TorshiaContext db;

        public BaseController()
        {
            this.db = new TorshiaContext();
        }

        protected override IViewable View([CallerMemberName] string actionName = "")
        {
            if (this.Identity != null)
            {
                var user = this.Identity.Username;
                var role = this.db.Users.First(u => u.Username == user).Role;
                if (role == Role.User)
                {
                    this.Model.Data["LoggedIn"] = "block";
                    this.Model.Data["NotLoggedIn"] = "none";
                    this.Model.Data["Username"] = this.Identity.Username;
                    this.Model.Data["Admin"] = "none";
                }
                else if (role == Role.Admin)
                {
                    this.Model.Data["LoggedIn"] = "none";
                    this.Model.Data["NotLoggedIn"] = "none";
                    this.Model.Data["Username"] = this.Identity.Username;
                    this.Model.Data["Admin"] = "block";
                }
             

            }
            else
            {
                this.Model.Data["LoggedIn"] = "none";
                this.Model.Data["NotLoggedIn"] = "block";
                this.Model.Data["Admin"] = "none";
            }
            return base.View(actionName);
        }
        protected string UrlDecode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        protected bool AdminAuthorization()
        {
            if (this.Identity != null)
            {
                var user = this.Identity.Username;
                var role = this.db.Users.First(u => u.Username == user).Role;
                if (role == Role.Admin)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        protected bool UserAuthorization()
        {
            if (this.Identity != null)
            {
                var user = this.Identity.Username;
                var role = this.db.Users.First(u => u.Username == user).Role;
                if (role == Role.User)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
