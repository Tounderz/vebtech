using vebtech.DTO;
using vebtech.Models;

namespace vebtech.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> Get(PaginationParameters paginationParameters, SortParameters sortParameters);
        User Create(UserDTO user);
        User GetUser(int id);
        void Delete (int id);
        User Update(int id, UserDTO user);
        bool IsEmailExist(string email);
        Role CreateRole(int userId, string name);
    }
}
