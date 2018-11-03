using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Method;
using SIS.Framework.Security;
using System;
using System.Collections.Generic;
using System.Text;
using Torshia.Web.Controllers.Base;
using Torshia.Web.Services.Contracts;
using Torshia.Web.ViewModels;

namespace Torshia.Web.Controllers
{
    public class UsersController : BaseController
    {
        private IUsersService usersService;
        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }
        public IActionResult Login() => this.View();

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var userExists = this.usersService.UserExistsByUsernameAndPassword(model.Username, model.Password);
            if (!userExists)
            {
                return this.RedirectToAction("/users/register");
            }

            this.SignIn(new IdentityUser() { Username = model.Username });
            return this.RedirectToAction("/");
        }

        public IActionResult Register() => this.View();

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            this.usersService.RegisterUser(model.Username, model.Password, model.Email);
            this.SignIn(new IdentityUser() { Email = model.Email, Username = model.Username });
            return RedirectToAction("/");
        }

        public IActionResult Logout()
        {
            this.SignOut();
            return this.RedirectToAction("/");
        }
    }
}
