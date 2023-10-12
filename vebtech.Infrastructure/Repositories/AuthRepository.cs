using AutoMapper;
using Microsoft.EntityFrameworkCore;
using vebtech.Domain.Models;
using vebtech.Domain.Models.Configurations;
using vebtech.Domain.Models.DTO;
using vebtech.Infrastructure.Context;
using vebtech.Infrastructure.Repositories.Abstract;

namespace vebtech.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly ApplicationContext _context;
    private readonly JwtConfig _jwtConfig;
    private readonly Mapper _mapper;

    public AuthRepository(ApplicationContext context, JwtConfig jwtConfig, Mapper mapper)
    {
        _context = context;
        _jwtConfig = jwtConfig;
        _mapper = mapper;
    }

    public async Task<Admin?> SignIn(AdminDto adminDto)
    {
        var admin = await _context.Admins.FirstOrDefaultAsync(user => user.Email == adminDto.Email);
        return admin == null || !BCrypt.Net.BCrypt.Verify(adminDto.Password, admin.Password) ? null : admin;
    }

    public async Task<Admin?> SignUp(AdminDto adminDto)
    {
        var admin = _mapper.Map<Admin>(adminDto);

        await _context.Admins.AddAsync(admin);
        await _context.SaveChangesAsync();
        return admin;
    }

    public async Task<bool> IsExistEmail(string email)
    {
        return await _context.Admins.FirstOrDefaultAsync(admin => admin.Email == email) != null;
    }
}