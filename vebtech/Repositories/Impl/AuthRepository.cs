using System.Linq.Dynamic.Core;
using vebtech.Context;
using vebtech.Models;

namespace vebtech.Repositories.Impl
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationContext _context;
        public AuthRepository(ApplicationContext context)
        {
            _context = context;
        }

        public bool IsExistEmail(string email)
        {
            return _context.Admins.First(admin => admin.Email == email) != null;
        }

        public Admin Signin(string email, string password)
        {
            return _context.Admins.FirstOrDefault(user => user.Email == email && user.Password == password);
        }

        public Admin Signup(string email, string password)
        {
            Admin admin = new Admin()
            {
                Email = email, 
                Password = password 
            };
            
            _context.Admins.Add(admin);
            _context.SaveChanges();
            return admin;
        }
    }
}
