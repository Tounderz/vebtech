using Microsoft.EntityFrameworkCore;
using vebtech.Context;
using vebtech.DTO;
using vebtech.Models;
using System.Linq.Dynamic.Core;
using vebtech.Utils;
using vebtech.CustomException;
using System.Net;
using vebtech.ConstParameters;
using vebtech.Enum;

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
            var user = GetUser(userId);
            var role = new Role()
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

        public IEnumerable<User> Get(PaginationParameters paginationParameters, 
            SortParameters sortParameters, FilterParameters filterParameters)
        {
            IEnumerable<User> users = _context.Users.Include(user => user.Roles).AsQueryable();

            if (filterParameters != null)
            {
                users =  Filter(users, filterParameters);
            }

            if (!string.IsNullOrEmpty(sortParameters.OrderBy))
            {
                if (sortParameters.OrderAsc != Enum.SortDirection.Ascending 
                    && sortParameters.OrderAsc != Enum.SortDirection.Descending )
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest, "Not valid sort order");
                }

                users = Sort(users, sortParameters);
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

        private IEnumerable<User> Sort(IEnumerable<User> users, SortParameters sortParameters)
        {
            switch (sortParameters.OrderBy.ToLower())
            {
                case "id":
                    return sortParameters.OrderAsc == SortDirection.Ascending
                        ? users.OrderBy(i => i.Id)
                        : users.OrderByDescending(i => i.Id);
                case "name":
                    return sortParameters.OrderAsc == SortDirection.Ascending
                        ? users.OrderBy(i => i.Name)
                        : users.OrderByDescending(i => i.Name);
                case "age":
                    return sortParameters.OrderAsc == SortDirection.Ascending
                        ? users.OrderBy(i => i.Age)
                        : users.OrderByDescending(i => i.Age);
                case "email":
                    return sortParameters.OrderAsc == SortDirection.Ascending
                        ? users.OrderBy(i => i.Email)
                        : users.OrderByDescending(i => i.Email);
                default:
                    throw new HttpResponseException(HttpStatusCode.BadRequest, "Not valid sort field");
            }
        }

        private IEnumerable<User> Filter(IEnumerable<User> users, FilterParameters filterParameters)
        {
            if (filterParameters.SearchQuery != null)
            {
                users = users.Where(user => user.Name.Contains(filterParameters.SearchQuery)
                || user.Email.Contains(filterParameters.SearchQuery) 
                || user.Age.ToString().Contains(filterParameters.SearchQuery)
                || (user.Roles != null && user.Roles.Any(role => role.Name.Contains(filterParameters.SearchQuery))));
            }

            if (filterParameters.Name != null)
            {
                users = users.Where(user => user.Name.Contains(filterParameters.Name));
            }

            if (filterParameters.Age != null)
            {
                users = users.Where(user => user.Age == filterParameters.Age);
            }

            if (filterParameters.Email != null)
            {
                users = users.Where(user => user.Email.Contains(filterParameters.Email));
            }

            if (filterParameters.RoleName != null)
            {
                users = users.Where(user => user.Roles.Any(role => role.Name.Contains(filterParameters.RoleName)));
            }

            return users;
        }
    }
}
