using vebtech.Domain.Models;
using vebtech.Domain.Models.DTO;

namespace vebtech.Application.Services.Interfaces;

public interface IUserService
{
    IEnumerable<User>? GetUsers(PaginationParameters paginationParameters,
         SortParameters sortParameters, FilterParameters filterParameters);
    Task<User?> GetUser(int id);
    Task<User?> CreateUser(UserDto userDto);
    Task<User?> UpdateUser(int id, UserDto userDto);
    Task<User?> DeleteUser(int id);
    Task<bool> IsEmailExist(string email);
    Task<Role?> CreateRole(RoleDto roleDto);
}