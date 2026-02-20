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
        public PlayerInfo(string name, PlayerType playerType, PlayerColor color, int meeplesCount)
        {
            Name = name;
            PlayerType = playerType;
            Color = color;
            MeeplesCount = meeplesCount;
        }

        public string Name { get; }
        public PlayerType PlayerType { get;  }
        public PlayerColor Color { get; }
        public int MeeplesCount { get; set; }
    }

    /// <summary>
    /// Players proprties in particular room
    /// </summary>
    public class GamePlayer
    {
        private List<Meeple> MeepleList { get; set; } = new List<Meeple>();

        public GamePlayer(PlayerInfo info)
        {
            Info = info;

            for (var i = 0; i < info.MeeplesCount; i++)
            {
                var meeple = new Meeple(this);
                MeepleList.Add(meeple);
            }
        }

        public PlayerInfo Info { get; set; }

        public bool IsAIProcessing {  get; set; }

        public Meeple? TakeMeeple()
        {
            if (MeepleList.Count == 0)
                return null;

            var meeple = MeepleList[0];
            MeepleList.Remove(meeple);
            Info.MeeplesCount = Info.MeeplesCount - 1;
            return meeple;
        }

        public void ReturnMeepleAndSetFlag(ObjectPart part)
        {
            if (part.Meeple == null)
                throw new Exception("Объект не принадлежит игроку, невозможно установить флаг.");

            var meeple = part.Meeple;
            meeple.Owner = this;
            MeepleList.Add(meeple);
            Info.MeeplesCount = Info.MeeplesCount + 1;

            part.Meeple = null;
            part.Flag = new Flag(this);
        }

        public void ProcessMove(GameRoom room)
        {
            if (room == null)
                throw new Exception("The room cannot be null.");

            IsAIProcessing = true;

            // where to put a card
            List<GameMove> possibleMoves = room.GetAvailableMoves();
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
                var gameCopy1 = room.Copy();
                gameCopy1.MakeMove(move);
                var expectedScore = gameCopy1.GetPlayerScore(this.Info.Name);
                dic.Add(move, expectedScore);
            }

            // ходы возвращающие фишки
            var maxReturnMeeples = dic.Max(item => (item.Value.MeeplesCount - MeepleList.Count));
            var returnMeeplesMoves = dic
                .Where(item => (item.Value.MeeplesCount - MeepleList.Count) == maxReturnMeeples);
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
