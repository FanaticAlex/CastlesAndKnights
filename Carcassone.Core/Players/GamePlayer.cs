using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
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
            chip.OwnerName = Name;
            СhipList.Add(chip);

            part.Chip = null;
            part.Flag = new Flag(this);
        }

        public void ProcessMove(GameRoom room)
        {
            if (room == null)
                throw new Exception("The room cannot be null.");

            var card = room.CardsPool.CurrentCard;
            if (card == null)
                return; // игра уже окончена

            IsAIProcessing = true;

            // where to put a card
            var availableFields = room.GetFieldsToPutCard(card.Id).Select(item => item.Id).ToList();
            List<GameMove> possibleMoves = GetPossibleMoves(room.Save(), card.Id, availableFields);
            GameMove bestMove = GetBestMove(possibleMoves);
            room.MakeMove(bestMove);

            IsAIProcessing = false;
        }

        private GameMove GetBestMove(List<GameMove> possibleMoves)
        {
            if (possibleMoves == null || possibleMoves.Count == 0)
                throw new Exception("AI cant make a move. No possible moves.");

            // ходы возвращающие фишки
            var maxReturnChips = possibleMoves.Max(m => (m.ExpectedScore.ChipCount - СhipList.Count));
            var returnChipsMove = possibleMoves
                .Where(m => (m.ExpectedScore.ChipCount - СhipList.Count) == maxReturnChips)
                .FirstOrDefault();
            if (maxReturnChips > 0 && returnChipsMove != null)
                return returnChipsMove;

            // ходы дающие наибольшее число очков
            var maxScore = possibleMoves.Max(m => m.ExpectedScore.GetOverallScore());
            var bestScoreMove = possibleMoves
                .Where(m => m.ExpectedScore.GetOverallScore() == maxScore)
                .First();
            return bestScoreMove;
        }

        private List<GameMove> GetPossibleMoves(string roomSave, string cardId, List<string> fieldIds)
        {
            var maxCalculations = 10;
            switch (PlayerType)
            {
                case PlayerType.AI_Easy: maxCalculations = 5; break;
                case PlayerType.AI_Normal: maxCalculations = 25; break;
                case PlayerType.AI_Hard: maxCalculations = 200; break;
            }

            var possibleMoves = new List<GameMove>();
            foreach (var fieldId in fieldIds)
            {
                var gameCopy = new GameRoom();
                gameCopy.Load(roomSave);
                var card = gameCopy.CardsPool.GetCard(cardId);
                var field = gameCopy.FieldBoard.GetField(fieldId);
                if (gameCopy.RotateCardTilFit(field, card))
                {
                    gameCopy.PutCardInField(card, field);

                    // where to put a chip
                    var partNames = gameCopy.GetAvailableParts(card.Id).Select(p => p.PartName);
                    foreach (var partName in partNames)
                    {
                        var gameCopy1 = new GameRoom();
                        gameCopy1.Load(gameCopy.Save());
                        var card1 = gameCopy1.CardsPool.GetCard(cardId);
                        var field1 = gameCopy1.FieldBoard.GetField(fieldId);
                        var part1 = card1.GetPart(partName);
                        if (!part1.IsPartOfOwnedObject && СhipList.Count > 0)
                        {
                            gameCopy1.PutChipInCard(part1, Name);
                            gameCopy1.ScoreCalculator.CloseObjectsAndReturnChips(gameCopy1.PlayersPool, gameCopy1.CardsPool);
                        }

                        var gameMove1 = new GameMove()
                        {
                            CardId = card.Id,
                            CardRotation = card.RotationsCount,
                            PlayerName = Name,
                            FieldId = field.Id,
                            PartName = part1.PartName,
                            ExpectedScore = gameCopy1.GetPlayerScore(this)
                        };

                        possibleMoves.Add(gameMove1);
                    }

                    gameCopy.ScoreCalculator.CloseObjectsAndReturnChips(gameCopy.PlayersPool, gameCopy.CardsPool);
                    // ход без установки фишки
                    var gameMove = new GameMove()
                    {
                        CardId = card.Id,
                        CardRotation = card.RotationsCount,
                        PlayerName = Name,
                        FieldId = field.Id,
                        PartName = null,
                        ExpectedScore =  gameCopy.GetPlayerScore(this)
                    };

                    possibleMoves.Add(gameMove);
                }

                if (possibleMoves.Count >= maxCalculations) // ограничение по времени выполнения
                    return possibleMoves;
            }

            return possibleMoves;
        }
    }
}
