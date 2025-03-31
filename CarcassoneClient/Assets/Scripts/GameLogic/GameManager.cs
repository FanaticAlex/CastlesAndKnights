using Carcassone.Core;
using Carcassone.Core.Players;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Assets.Scripts
{
    /// <summary>
    /// Отвечает за создание синглтона сервиса игры 
    /// </summary>
    internal sealed class GameManager
    {
        private static readonly Lazy<GameManager> _instance = new Lazy<GameManager>(() => new GameManager());

        
        private GameManager()
        {
            Room = new GameRoom();
            Players = PlayersManager.Load();
        }

        public static GameManager Instance => _instance.Value;

        public GameRoom Room { get; private set; }
        public List<Player> Players {  get; private set; }

        public Player GetDefaultPlayer()
        {
            return Players.Where(p => p.PlayerType == PlayerType.Human).FirstOrDefault();
        }

        public Player GetPlayer(string name)
        {
            return Players.Where(p => p.Name == name).FirstOrDefault();
        }

        public void AddPlayer(string playerName, PlayerType playerType)
        {
            var newPlayer = new Player() { Name = playerName, PlayerType = playerType };
            Players.Add(newPlayer);
            PlayersManager.Save(Players);
        }

        public void DeletePlayer(string playerName)
        {
            var deletedPlayer = Players.Single(p => p.Name == playerName);
            Players.Remove(deletedPlayer);
            PlayersManager.Save(Players);
        }
    }
}
