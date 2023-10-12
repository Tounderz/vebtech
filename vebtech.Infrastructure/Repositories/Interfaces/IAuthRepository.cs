using vebtech.Domain.Models;
using vebtech.Domain.Models.DTO;

namespace vebtech.Infrastructure.Repositories.Abstract;

public interface IAuthRepository
{
    Task<Admin?> SignIn(AdminDto adminDto);
    Task<Admin?> SignUp(AdminDto adminDto);
    Task<bool> IsExistEmail(string email);
}