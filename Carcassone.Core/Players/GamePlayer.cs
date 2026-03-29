using Carcassone.Core.Calculation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Carcassone.Core.Players
{
    public enum PlayerColor
    {
        Blue,
        Gray,
        Green,
        Red,
        //White,
        Yellow
    }

    public class PlayerInfo
    {
        public PlayerInfo(string name, PlayerType playerType, PlayerColor color)
        {
            Name = name;
            PlayerType = playerType;
            Color = color;
        }

        public string Name { get; }
        public PlayerType PlayerType { get;  }
        public PlayerColor Color { get; }
    }

    /// <summary>
    /// Players proprties in particular room
    /// </summary>
    public class GamePlayer
    {
        public const int MeeplesMaxCount = 7; 

        public GamePlayer(PlayerInfo info)
        {
            Info = info;
        }

        public PlayerInfo Info { get; set; }

        public bool IsAIProcessing {  get; set; }

        public Meeple? TakeMeeple()
        {
            return new Meeple(this);
        }

        public void ProcessMove(GameRoom room)
        {
            if (room == null)
                throw new Exception("The room cannot be null.");

            IsAIProcessing = true;

            // where to put a card
            var currentTile = room.GetCurrentTile();
            List<GameMove> possibleMoves = room.GetAvailableMoves(currentTile.Id);
            GameMove bestMove = GetBestMove(room, possibleMoves);
            room.MakeMove(bestMove);

            IsAIProcessing = false;
        }

        private GameMove GetBestMove(GameRoom room, List<GameMove> possibleMoves)
        {
            if (possibleMoves == null || possibleMoves.Count == 0)
                throw new Exception("AI cant make a move. No possible moves.");

            var dic = new Dictionary<GameMove, PlayerScore>();
            foreach (GameMove move in possibleMoves)
            {
                room.MakeMove(move);
                var expectedScore = room.GetPlayerScore(this.Info.Name);
                dic.Add(move, expectedScore);
                room.UndoLastMove();
            }

            // ходы возвращающие фишки
            var maxReturnMeeples = dic.Max(item => (item.Value.MeeplesCount - room.GetMeeplesCount(this)));
            var returnMeeplesMoves = dic.Where(item => (item.Value.MeeplesCount - room.GetMeeplesCount(this)) == maxReturnMeeples);
            if (maxReturnMeeples > 0 && returnMeeplesMoves.Count() > 0)
            {
                return returnMeeplesMoves.FirstOrDefault().Key;
            }

            // ходы дающие наибольшее число очков
            var maxScore = dic.Max(item => item.Value.OverallScore);
            var bestScoreMove = dic
                .Where(item => item.Value.OverallScore == maxScore)
                .First();
            return bestScoreMove.Key;
        }
    }
}
