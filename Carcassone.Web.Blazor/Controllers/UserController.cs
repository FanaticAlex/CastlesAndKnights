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
        public async Task<ActionResult<TokenResult>> Login(string login, [DataType(DataType.Password)] string password)
        {
            /* var user = await _userManager.FindByNameAsync(login);
            if (user?.UserName == null)
                return Unauthorized();

            if (!await _userManager.CheckPasswordAsync(user, password))
                return Unauthorized();

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            var secret = _configuration["JWT:Secret"];
            if (secret == null)
                return Problem();

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: null,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return Ok(new TokenResult
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });*/

            return null;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<PlayedGame>>> GetPlayedGameList()
        {
            _logger.LogInformation("GetScoreByUser Called");
            var user = await _userManager.GetUserAsync(User);
            return _service.GetPlayedGameList(user).ToList();
        }

        [HttpGet]
        [Route("user/{userName}/info")]
        public async Task<ActionResult<UserInfo>> GetUserStatisticAsync(string userName)
        {
            _logger.LogInformation("GetUserStatistic Called");
            var user = await _userManager.GetUserAsync(User);
            return _service.GetUserInfo(user);
        }
    }
}
