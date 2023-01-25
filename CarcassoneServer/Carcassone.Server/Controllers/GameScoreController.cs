using Carcassone.DAL;
using Carcassone.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameScoreController : ControllerBase
    {
        private IGameScoreService _service;

        public GameScoreController(IGameScoreService userService)
        {
            _service = userService;
        }

        [HttpGet]
        [Route("user/{userName}")]
        public List<GameScore> GetScoreByUser(string userName)
        {
            return _service.GetUserScores(userName).ToList();
        }

        [HttpGet]
        [Route("game/{gameId}")]
        public List<GameScore> GetScoreByGame(string gameId)
        {
            return _service.GetGameScores(gameId).ToList();
        }
    }
}
