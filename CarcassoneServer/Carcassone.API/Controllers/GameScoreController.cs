using Carcassone.DAL;
using Carcassone.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;

namespace Carcassone.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class GameScoreController : ControllerBase
    {
        private readonly IGameScoreService _service;
        private readonly ILogger _logger;

        public GameScoreController(
            ILogger<GameScoreController> logger,
            IGameScoreService userService)
        {
            _service = userService;
            _logger = logger;
        }

        [HttpGet]
        [Route("user/{userName}")]
        public ActionResult<List<UserGameScore>> GetScoreByUser(string userName)
        {
            _logger.LogInformation("GetScoreByUser Called");
            //return _service.GetUserScores(userName).ToList();
            return new List<UserGameScore>();
        }

        [HttpGet]
        [Route("game/{gameId}")]
        public ActionResult<List<UserGameScore>> GetScoreByGame(string gameId)
        {
            _logger.LogInformation("GetScoreByGame Called");
            //return _service.GetGameScores(gameId).ToList();
            return new List<UserGameScore>();
        }

        [HttpGet]
        [Route("user/{userName}/statistic")]
        public ActionResult<UserStatistic> GetUserStatistic(string userName)
        {
            _logger.LogInformation("GetUserStatistic Called");
            //return _service.GetUserStatistic(userName);
            return new UserStatistic();
        }
    }
}
