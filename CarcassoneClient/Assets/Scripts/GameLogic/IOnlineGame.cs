using Assets.Scripts.Game;
using Carcassone.Core;
using Carcassone.Core.Calculation;
using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System.Collections.Generic;

namespace Assets.Scripts
{
    internal interface IOnlineGame
    {
        void Login(SavedAuthData data); 
        void Login(string login, string password); 
        void Connect(string roomId); 
        List<string> GetRoomsIds(); 
    }

    internal class GameLogic
    {
        private GameRoom _room;

        public GameLogic()
        {
            _room = new GameRoom(GameParameters.Instance.Extensions, GameParameters.Instance.Players);
        }

        public List<GamePlayer> GetPlayers() => _room.GetPlayers();
        public GamePlayer GetCurrentPlayer() => _room.GetCurrentPlayer();
        public IEnumerable<PlayerScore> GetPlayersScores() => _room.PlayersPool.GetPlayersScores(_room.State);
        public GamePlayer GetPlayer(string name) => _room.PlayersPool.GetPlayer(name);

        public bool IsFinished() => _room.IsFinished();

        public Tile GetTile(string tileId) => _room.GetTile(tileId);
        public Tile GetCurrentTile() => _room.GetCurrentTile();
        public int GetRemainTilesCount() => _room.GetRemainTilesCount();

        public List<GameMove> GetAvailableMoves(string tileId) => _room.GetAvailableMoves(tileId);
        public void MakeMove(GameMove gameMove) => _room.MakeMove(gameMove);

        public Locations GetCellsStatus() => _room.GetCellsStatus();

        public MoveState GetState() => _room.State;

        public void ProcessAIPlayerMove() => GetCurrentPlayer().ProcessMove(_room);
    }
}
