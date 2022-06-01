using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carcassone.DAL
{
    public class UserService : IUserService
    {
        private CarcassoneContext _context;

        public UserService(CarcassoneContext context)
        {
            _context = context;
        }

        public User Authorize(string login, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Login == login && x.Password == password);
            if (user == null)
                throw new Exception("Неверный логин или пароль");

            return user;
        }

        public void Register(User newUser)
        {
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        public User GetUser(string login)
        {
            return _context.Users.FirstOrDefault(x => x.Login == login);
        }
    }
}
