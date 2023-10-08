using vebtech.Models;
using vebtech.Models.DTO;

namespace vebtech.Repositories.Abstract;

public interface IAuthRepository
{
    Task<Admin> SignIn(AdminDto adminDto);
    Task<Admin> SignUp(AdminDto adminDto);
    Task<bool> IsExistEmail(string email);
    Task<string> GenerateJwt(AdminDto adminDto);
}