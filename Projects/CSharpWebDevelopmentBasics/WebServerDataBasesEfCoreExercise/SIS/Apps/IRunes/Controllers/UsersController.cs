using CakesWebApp.Services;
using IRunes.Models;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace IRunes.Controllers
{
    public class UsersController : BaseController
    {
        private readonly HashService hashService;


        public UsersController()
        {
            this.hashService = new HashService();
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                return new RedirectResult("/");
            }
            return this.View();
        }


        public IHttpResponse PostLogin(IHttpRequest request)
        {


            var usernameOrEmail = request.FormData["username"].ToString();
            var password = request.FormData["password"].ToString();

            var hashedPassword = hashService.Hash(password);

            User user;

            if (usernameOrEmail.Contains("%40"))
            {
                WebUtility.UrlEncode(usernameOrEmail);

                user = Context.Users.FirstOrDefault(x => x.Email == usernameOrEmail && x.Password == hashedPassword);
            }
            else
            {
                user = this.Context.Users.FirstOrDefault(x => x.Username == usernameOrEmail && x.Password == hashedPassword);
            }
            if (user == null)
            {
                return new RedirectResult("/Users/Login");
            }

            var username = user.Username;

            var response = new RedirectResult("/Home/Index");

            this.SignInUser(username, request, response);

            return response;
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                return new RedirectResult("/");
            }
            return this.View();
        }

        public IHttpResponse PostRegister(IHttpRequest request)
        {
            var userName = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();
            var confirmPassword = request.FormData["confirmPassword"].ToString();
            var email = request.FormData["email"].ToString();

            //Validate
            if (string.IsNullOrWhiteSpace(userName) || userName.Length < 4)
            {
                return new BadRequestResult("Please provide valid username with length of 4 or more characters.", HttpResponseStatusCode.Unauthorized);
            }

            if (this.Context.Users.Any(x => x.Username == userName))
            {
                return new BadRequestResult("User with the same name already exists.", HttpResponseStatusCode.Unauthorized);
            }
            if (this.Context.Users.Any(x => x.Email == email))
            {
                return new BadRequestResult("User with the same Email already exists.", HttpResponseStatusCode.Unauthorized);
            }
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return new BadRequestResult("Please provide password of length 6 or more.", HttpResponseStatusCode.Unauthorized);
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                return new BadRequestResult("Please add an Email.", HttpResponseStatusCode.Unauthorized);
            }

            if (password != confirmPassword)
            {
                return new BadRequestResult("Passwords do not match.", HttpResponseStatusCode.SeeOther);
            }

            // Hash password
            var hashedPassword = this.hashService.Hash(password);

            // Create user
            var user = new User
            {
                Username = userName,
                Password = hashedPassword,
                Email = email
            };
            this.Context.Users.Add(user);

            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception e)
            {
                // TODO: Log error
                return new BadRequestResult(e.Message, HttpResponseStatusCode.InternalServerError);
            }

            // TODO: Login
            var response = new RedirectResult("/");
            this.SignInUser(userName, request, response);
            // Redirect
            return response;
        }

        public IHttpResponse Logout(IHttpRequest request)
        {

            this.SignOutUser(request);

            return new RedirectResult("/");
        }

    }
}
