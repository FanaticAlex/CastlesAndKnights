using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carcassone.DAL
{
    public interface IUserService
    {
        public void Register(User newUser);
        public User Authorize(string login, string password);
        public User GetUser(string login);
    }
}
