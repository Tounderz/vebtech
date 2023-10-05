using Microsoft.EntityFrameworkCore;
using vebtech.Context;
using vebtech.DTO;
using vebtech.Models;
using System.Linq.Dynamic.Core;
using vebtech.Utils;
using vebtech.CustomException;
using System.Net;

namespace vebtech.Repositories.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public User Create(UserDTO userDTO)
        {
            var user = new User()
            {
                Name = userDTO.Name,
                Age = (int)userDTO.Age,
                Email = userDTO.Email
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public Role CreateRole(int userId, string name)
        {
            User user = GetUser(userId);
            Role role = new Role()
            {
                Name = name,
                User = user,
            };

            _context.Roles.Add(role);
            _context.SaveChanges();
            return role;
        }

        public void Delete(int id)
        {
            _context.Users.Remove(this.GetUser(id));
            _context.SaveChanges();
        }

        public IEnumerable<User> Get(PaginationParameters paginationParameters, SortParameters sortParameters)
        {
            var users = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(sortParameters.OrderBy))
            {
                string orderFlow = sortParameters.OrderAsc ? "ascending" : "descending";
                users = users.OrderBy($"{sortParameters.OrderBy} {orderFlow}");
            }

            return users
                .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize);
        }

        public User GetUser(int id)
        {
            User user = _context.Users.FirstOrDefault(user => user.Id == id);

            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, "User not found");
            }

            return user;
        }

        public bool IsEmailExist(string email)
        {
            return _context.Users.FirstOrDefault(user => user.Email == email) != null;
        }

        public User Update(int id, UserDTO user)
        {
            User oldUser = GetUser(id);
            if (oldUser == null)
            {
                return null;
            }

            oldUser.Name = user.Name ?? oldUser.Name;
            oldUser.Age = user.Age != null ? (int)user.Age : oldUser.Age;
            oldUser.Email = user.Email ?? oldUser.Email;

            _context.Entry(oldUser).State = EntityState.Modified;
            _context.SaveChanges();
            return oldUser;
        }
    }
}
