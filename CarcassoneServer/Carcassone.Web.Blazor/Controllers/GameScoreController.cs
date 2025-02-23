using Carcassone.Web.Blazor.Data;
using Carcassone.Web.Blazor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Carcassone.Web.Blazor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class GameScoreController : ControllerBase
    {
        private readonly IPlayedGameStore _service;
        private readonly ILogger _logger;
        private readonly UserManager<CarcassoneUser> _userManager;

        public GameScoreController(
            ILogger<GameScoreController> logger,
            IPlayedGameStore userService)
        {
            _service = userService;
            _logger = logger;
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
        [Route("user/{userName}/statistic")]
        public async Task<ActionResult<UserStatistic>> GetUserStatisticAsync(string userName)
        {
            _logger.LogInformation("GetUserStatistic Called");
            var user = await _userManager.GetUserAsync(User);
            return _service.GetStatistic(user);
        }
    }
}
