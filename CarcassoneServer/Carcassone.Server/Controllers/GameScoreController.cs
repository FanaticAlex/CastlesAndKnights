using Carcassone.DAL;
using Carcassone.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameScoreController : ControllerBase
    {
        private IGameScoreService _service;
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
            try
            {
                _logger.LogWarning("GetScoreByUser Called");
                return _service.GetUserScores(userName).ToList();
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return Problem(e.Message);
            }
        }

        [HttpGet]
        [Route("game/{gameId}")]
        public ActionResult<List<UserGameScore>> GetScoreByGame(string gameId)
        {
            try
            {
                return _service.GetGameScores(gameId).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Problem(e.Message);
            }
        }

        [HttpGet]
        [Route("user/{userName}/statistic")]
        public ActionResult<UserStatistic> GetUserStatistic(string userName)
        {
            try
            {
                return _service.GetUserStatistic(userName);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Problem(e.Message);
            }
        }
    }
}
