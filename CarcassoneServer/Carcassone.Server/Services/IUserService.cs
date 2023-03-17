using Carcassone.DAL;

namespace Carcassone.Server.Services
{
    public interface IUserService
    {
        public void Register(User newUser);
        public User Authorize(string login, string password);
        public User GetUser(string login);
    }
}
