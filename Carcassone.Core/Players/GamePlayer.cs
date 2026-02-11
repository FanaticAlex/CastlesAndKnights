using Carcassone.Core.Calculation;
using Carcassone.Core.Board;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Carcassone.Core.Tiles;
using System.Drawing;

namespace Carcassone.Core.Players
{
    public class PlayerInfo
    {
        public PlayerInfo(string name, PlayerType playerType, string color, int meeplesCount)
        {
            Name = name;
            PlayerType = playerType;
            Color = color;
            MeeplesCount = meeplesCount;
        }

        public string Name { get; }
        public PlayerType PlayerType { get;  }
        public string Color { get; }
        public int MeeplesCount { get; set; }
    }

    /// <summary>
    /// Players proprties in particular room
    /// </summary>
    public class GamePlayer
    {
        private List<Chip> СhipList { get; set; } = new List<Chip>();

        public GamePlayer(PlayerInfo info)
        {
            Info = info;

            for (var i = 0; i < info.MeeplesCount; i++)
            {
                var chip = new Chip(this);
                СhipList.Add(chip);
            }
        }

        public PlayerInfo Info { get; set; }

        public bool IsAIProcessing {  get; set; }

        public Chip? TakeChip()
        {
            if (СhipList.Count == 0)
                return null;

            var chip = СhipList[0];
            СhipList.Remove(chip);
            Info.MeeplesCount = Info.MeeplesCount - 1;
            return chip;
        }

        public void ReturnChipAndSetFlag(ObjectPart part)
        {
            if (part.Chip == null)
                throw new Exception("Объект не принадлежит игроку, невозможно установить флаг.");

            var chip = part.Chip;
            chip.Owner = this;
            СhipList.Add(chip);
            Info.MeeplesCount = Info.MeeplesCount + 1;

            part.Chip = null;
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
            var maxReturnChips = dic.Max(item => (item.Value.ChipCount - СhipList.Count));
            var returnChipsMoves = dic
                .Where(item => (item.Value.ChipCount - СhipList.Count) == maxReturnChips);
            if (maxReturnChips > 0 && returnChipsMoves.Count() > 0)
            {
                return returnChipsMoves.FirstOrDefault().Key;
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
