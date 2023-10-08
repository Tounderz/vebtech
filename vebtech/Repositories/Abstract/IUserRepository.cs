using vebtech.Models;
using vebtech.Models.DTO;

namespace vebtech.Repositories.Abstract;

public interface IUserRepository
{
    IEnumerable<User> Get(PaginationParameters paginationParameters,
        SortParameters sortParameters, FilterParameters filterParameters);
    Task<User> Create(UserDto userDto);
    Task<User> GetUser(int id);
    Task Delete(int id);
    Task<User> Update(int id, UserDto userDto);
    Task<bool> IsEmailExist(string email);
    Task<Role> CreateRole(int userId, string name);
}