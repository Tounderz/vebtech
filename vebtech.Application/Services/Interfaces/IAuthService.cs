using vebtech.Domain.Models.DTO;
using vebtech.Domain.Models;

namespace vebtech.Application.Services.Interfaces;

public interface IAuthService
{
    Task<Admin?> SignIn(AdminDto adminDto);
    Task<Admin?> SignUp(AdminDto adminDto);
    Task<bool> IsExistEmail(string email);
}