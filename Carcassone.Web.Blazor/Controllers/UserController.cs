using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Carcassone.Core;
using Carcassone.Web.Blazor.Controllers;
using Carcassone.Web.Blazor.Data;
using Carcassone.Web.Blazor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
//using Microsoft.IdentityModel.Tokens;

namespace CarcassoneServer.Web.Blazor.Controllers
{
    public class TokenResult
    {
        public string Token { get; set; }
        public DateTime? Expiration { get;set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<CarcassoneUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IPlayedGameStore _service;
        private readonly ILogger _logger;

        public UserController(
            ILogger<UserController> logger,
            IPlayedGameStore userService,
            UserManager<CarcassoneUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _service = userService;
            _logger = logger;
        }

        [HttpGet]
        [Route("login/{login}/{password}")]
        [AllowAnonymous]
        public ActionResult<TokenResult> Login(string login, [DataType(DataType.Password)] string password)
        {
            throw new NotImplementedException();
        }
    }
}
