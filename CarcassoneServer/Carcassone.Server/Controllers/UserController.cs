using System;
using Carcassone.DAL;
using Carcassone.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarcassoneServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("register/{email}/{login}/{password}")]
        public void Register(string login, string password, string email)
        {
            var newUser = new User()
            {
                Login = login,
                Password = password,
                Email = email,
            };

            _userService.Register(newUser);
        }

        [HttpGet]
        [Route("verify/{key}")]
        public void Verify(string key)
        {
            throw new Exception();
        }

        [HttpGet]
        [Route("login/{login}/{password}")]
        public User Login(string login, string password)
        {
            var user = _userService.Authorize(login, password);
            return user;
        }
    }
}
