using Carcassone.Core;
using Carcassone.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace Assets.Scripts
{
    /// <summary>
    /// Отвечает за создание синглтона сервиса игры 
    /// </summary>
    internal sealed class GameParameters
    {
        private static GameParameters instance;

        public static GameParameters Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameParameters();
                return instance;
            }
        }

        private GameParameters()
        {
            var players = PlayersManager.Load();
            foreach (var player in players)
            {
                var newPlayer = new PlayerInfo(player.Name, player.Type, GetAvailableColor(Players), GamePlayersPool.PlayerMeeplesCount);
                Players.Add(newPlayer);
            }
        }

        public List<PlayerInfo> Players {  get; private set; } = new List<PlayerInfo>();
        public List<Extension> Extensions { get; private set; } = new List<Extension>();

        public PlayerInfo GetDefaultPlayer()
        {
            return Players.Where(p => p.PlayerType == PlayerType.Human).FirstOrDefault();
        }

        public PlayerInfo GetPlayer(string name)
        {
            return Players.Where(p => p.Name == name).FirstOrDefault();
        }

        public void AddPlayer(string playerName, PlayerType playerType)
        {
            var newPlayer = new PlayerInfo(playerName, playerType, PlayerColor.Red, GamePlayersPool.PlayerMeeplesCount);
            Players.Add(newPlayer);
        }

        public void DeletePlayer(string playerName)
        {
            var deletedPlayer = Players.Single(p => p.Name == playerName);
            Players.Remove(deletedPlayer);
        }

        public void SaveScore()
        {
            /*var gamePLayers = Room.PlayersPool.GamePlayers.Where(p => p.PlayerType != PlayerType.NetworkPlayer);
            foreach (var gamePlayer in gamePLayers)
            {
                var score = Room.GetPlayerScore(gamePlayer.Name);
                var player = Players.Single(p => p.Name == gamePlayer.Name);

                player.GamesCount = player.GamesCount + 1;

                if (score.Rank == 0)
                    player.WinCount = player.WinCount + 1;
            }

            PlayersManager.Save(Players);*/
        }

        public void ResetGame()
        {
            //Room = new GameRoom();
        }

        public PlayerColor GetAvailableColor(List<PlayerInfo> players)
        {
            var takenColors = players.Select(p => p.Color).ToList();
            foreach(var color in Enum.GetValues(typeof(PlayerColor)))
            {
                if (takenColors.Contains((PlayerColor)color))
                    continue;

                return (PlayerColor)color;
            }

            throw new Exception("There is no free colors left");
        }
    }
}
