using vebtech.Models;

namespace vebtech.Repositories
{
    public interface IAuthRepository
    {
        Admin Signin(string email, string password);
        bool IsExistEmail(string email);
        Admin Signup(string email, string password);
    }
}
