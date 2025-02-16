using Carcassone.Core;
using Carcassone.DAL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.DAL.Services
{
    public class PlayedGameStore : IPlayedGameStore
    {
        private CarcassoneContext _context;
        private UserManager<CarcassoneUser> _userManager;

        public PlayedGameStore(CarcassoneContext context, UserManager<CarcassoneUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IEnumerable<PlayedGame> GetPlayedGameList(CarcassoneUser user)
        {
            return _context.PlayedGameList
                .Where(g => g.PlayerFinalResultList.Select(r => r.CarcassoneUser).Contains(user));
        }

        public UserStatistic GetStatistic(CarcassoneUser user)
        {
            var playerFinalResultList = _context.PlayerFinalResultList.Where(r => r.CarcassoneUser == user);
            var gamesCount = playerFinalResultList.Count();
            var winCount = playerFinalResultList.Where(g => g.Rank == 0).Count();

            var statistic = new UserStatistic()
            {
                GamesCount = gamesCount,
                WinCount = winCount
            };

            return statistic;
        }

        public async Task AddGameResults(GameRoom room)
        {
            var playedGame = new PlayedGame();
            foreach (var player in room.PlayersPool.Players)
            {
                var playerScore = room.GetPlayerScore(player);
                var playerResult = new PlayerFinalResult()
                {
                    CarcassoneUser = await _userManager.FindByNameAsync(player.Name),
                    FinalScore = playerScore.GetOverallScore(),
                    Rank = playerScore.Rank
                };

                _context.PlayerFinalResultList.Add(playerResult);
                playedGame.PlayerFinalResultList.Add(playerResult);
            }

            _context.PlayedGameList.Add(playedGame);
            _context.SaveChanges();
        }
    }
}
