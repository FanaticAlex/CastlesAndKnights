using Carcassone.Core.Calculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Carcassone.Core.Players
{
    public static class ListHelper
    {
        public static bool HasDuplicates<T>(this List<T> list)
        {
            // If the count of distinct items is less than the total count, there are duplicates.
            return list.Count() != list.Distinct().Count();
        }
    }

    /// <summary>
    /// Класс управляющий пулом игроков одной игровой комнаты.
    /// </summary>
    public class GamePlayersPool
    {
        private static readonly int _maximumPlayersCount = 5;

        public List<GamePlayer> GamePlayers { get; } = new List<GamePlayer>();
        public int CurrentPlayerIndex { get; set; }

        public GamePlayersPool(List<PlayerInfo> playerInfos)
        {
            if (playerInfos.Count >= _maximumPlayersCount)
                throw new Exception($"Maximum player count is {_maximumPlayersCount}");

            if (playerInfos.Select(i => i.Name).ToList().HasDuplicates())
                throw new Exception($"There is a duplicate player's name");

            if (playerInfos.Select(i => i.Color).ToList().HasDuplicates())
                throw new Exception($"There is a duplicate player's color");

            foreach (var playerInfo in playerInfos)
            {
                GamePlayers.Add(new GamePlayer(playerInfo));
            }
        }

        public IEnumerable<PlayerScore> GetPlayersScores(MoveState state)
        {
            // calculating ranks by overall score
            var list = new List<Tuple<GamePlayer, int>>();
            foreach (var player in GamePlayers)
                list.Add(new Tuple<GamePlayer, int>(player, player.GetPlayerOverallScore(state)));

            list.Sort((x, y) => y.Item2.CompareTo(x.Item2));

            // calculating scores
            var scores = new List<PlayerScore>();
            foreach (var item in list)
            {
                var player = item.Item1;
                var meeples = player.GetMeeplesCount(state);
                var overallscore = item.Item2;
                var rank = list.IndexOf(item);
                var score = new PlayerScore(player.Info.Name, meeples, rank, overallscore, state);
                scores.Add(score);
            }

            return scores;
        }

        public GamePlayer? GetPlayer(string playerName)
        {
            if (string.IsNullOrEmpty(playerName)) return null;

            var playerItems = GamePlayers.Where(_player => _player.Info.Name == playerName);
            if (playerItems.Count() == 0)
                return null;

            if (playerItems.Count() > 1)
                throw new Exception($"Duplicate players {playerName} in PlayersPool");

            return playerItems.Single();
        }

        public GamePlayer GetCurrentPlayer() => GamePlayers[CurrentPlayerIndex];

        public void MoveToNextPlayer()
        {
            CurrentPlayerIndex++;
            CurrentPlayerIndex %= GamePlayers.Count;
        }

        public void MoveToPreviousPlayer()
        {
            CurrentPlayerIndex--;
            if (CurrentPlayerIndex < 0) CurrentPlayerIndex = GamePlayers.Count - 1;
        }
    }
}
