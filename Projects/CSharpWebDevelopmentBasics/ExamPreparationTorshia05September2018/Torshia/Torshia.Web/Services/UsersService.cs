using System.Linq;
using Torshia.Data;
using Torshia.Models;
using Torshia.Models.Enums;
using Torshia.Web.Services.Contracts;

namespace Torshia.Web.Services
{
    public class UsersService : IUsersService
    {
        private readonly TorshiaContext context;

        public UsersService(TorshiaContext context)
        {
            this.context = context;
        }
        public void RegisterUser(string username, string password, string email)
        {
            var role = this.context.Users.Any() ? Role.User : Role.Admin;
            var user = new User()
            {
                Username = username,
                Email = email,
                Password = password,
                Role = role
            };
            this.context.Users.Add(user);
            this.context.SaveChanges();
        }

        public bool UserExistsByUsernameAndPassword(string username, string password) =>
            this.context.Users.Any(u => u.Username == username && u.Password == password);
    }
}
