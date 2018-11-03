namespace Torshia.Web.Services.Contracts
{
    public interface IUsersService
    {
        void RegisterUser(string username, string password, string email);

        bool UserExistsByUsernameAndPassword(string username, string password);

    }
}
