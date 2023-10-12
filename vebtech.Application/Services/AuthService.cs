using vebtech.Application.Services.Interfaces;
using vebtech.Domain.Models;
using vebtech.Domain.Models.DTO;
using vebtech.Infrastructure.Repositories.Abstract;

namespace vebtech.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;

    public AuthService(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public async Task<Admin?> SignIn(AdminDto adminDto)
    {
        return await _authRepository.SignIn(adminDto);
    }

    public async Task<Admin?> SignUp(AdminDto adminDto)
    {
        return await _authRepository.SignUp(adminDto);
    }

    public async Task<bool> IsExistEmail(string email)
    {
        return await _authRepository.IsExistEmail(email);
    }
}