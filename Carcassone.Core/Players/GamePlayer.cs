using Carcassone.Core.Calculation;
using Carcassone.Core.Board;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Carcassone.Core.Players
{
    /// <summary>
    /// Players proprties in particular room
    /// </summary>
    public class GamePlayer
    {
        public GamePlayer(string _name, PlayerType _type, string color, int chipCount)
        {
            Name = _name;
            PlayerType = _type;
            Color = color;

            for (var i = 0; i < chipCount; i++)
            {
                var chip = new Chip(this);
                СhipList.Add(chip);
            }
        }

        public string Name { get; set; }
        public PlayerType PlayerType { get; set; }
        public string Color { get; set; }
        public List<Chip> СhipList { get; set; } = new List<Chip>();

        public bool IsAIProcessing {  get; set; }

        public Chip? TakeChip()
        {
            if (СhipList.Count == 0)
                return null;

            var chip = СhipList[0];
            СhipList.Remove(chip);
            return chip;
        }

        public void ReturnChipAndSetFlag(ObjectPart part)
        {
            if (part.Chip == null)
                throw new Exception("Объект не принадлежит игроку, невозможно установить флаг.");

            var chip = part.Chip;
            chip.Owner = this;
            СhipList.Add(chip);

            part.Chip = null;
            part.Flag = new Flag(this);
        }

        public void ProcessMove(GameRoom room)
        {
            if (room == null)
                throw new Exception("The room cannot be null.");

            var card = room.TileStack.CurrentCard;
            if (card == null)
                return; // игра уже окончена

            IsAIProcessing = true;

            // where to put a card
            List<GameMove> possibleMoves = GetPossibleMoves(room, card.Id);
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
                var gameCopy1 = new GameRoom();
                gameCopy1.Load(room.Save());
                gameCopy1.MakeMove(move);
                var expectedScore = gameCopy1.GetPlayerScore(this.Name);
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

        private List<GameMove> GetPossibleMoves(GameRoom room, string tileId)
        {
            var roomSave = room.Save();
            var availableCells = room.GetCellsToPutCard(tileId).Select(item => item.Id).ToList();

            var maxCalculations = 10;
            switch (PlayerType)
            {
                case PlayerType.AI_Easy: maxCalculations = 5; break;
                case PlayerType.AI_Normal: maxCalculations = 25; break;
                case PlayerType.AI_Hard: maxCalculations = 200; break;
            }

            var possibleMoves = new List<GameMove>();
            foreach (var cellId in availableCells)
            {
                var gameCopy = new GameRoom();
                gameCopy.Load(roomSave);
                var tile = gameCopy.TileStack.GetTile(tileId);
                var cell = gameCopy.GameGrid.GetCell(cellId);
                
                if (!gameCopy.RotateTileTilFit(cell, tile)) // если карта не подходит
                    continue;


                gameCopy.AddTile(tile, cell); 

                // ходы с установкой фишки
                var partNames = gameCopy.GetAvailableParts(tile.Id).Select(p => p.PartName).ToList();
                partNames.Add(null); // ход без установки фишки
                foreach (var partName in partNames)
                {
                    var gameMove1 = new GameMove()
                    {
                        TileId = tile.Id,
                        TileRotation = tile.RotationsCount,
                        PlayerName = Name,
                        CellId = cell.Id,
                        PartName = partName,
                    };

                    possibleMoves.Add(gameMove1);
                }

                if (possibleMoves.Count >= maxCalculations) // ограничение по времени выполнения
                    return possibleMoves;
            }

            return possibleMoves;
        }
    }
}
