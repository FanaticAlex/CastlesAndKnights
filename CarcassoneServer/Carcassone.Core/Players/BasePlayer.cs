using Carcassone.Core.Cards;
using Carcassone.Core.Players.AI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Players
{
    public enum PlayerType
    {
        Human,
        AI_Easy,
        AI_Normal,
        AI_Hard
    }

    public class BasePlayer
    {
        private List<Chip> _chipList = new List<Chip>();

        public BasePlayer(string name, string color, int chipCount, PlayerType playerType)
        {
            Name = name;
            Color = color;
            PlayerType = playerType;

            for (var i = 0; i < chipCount; i++)
            {
                var chip = new Chip(this);
                _chipList.Add(chip);
            }
        }

        public PlayerType PlayerType { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public string? LastCardId { get; set; }

        public int ChipCount => _chipList.Count;

        public Chip? TakeChip()
        {
            if (_chipList.Count == 0)
                return null;

            var chip = _chipList[0];
            _chipList.Remove(chip);
            return chip;
        }

        public void ReturnChipAndSetFlag(ObjectPart part)
        {
            if (part.Chip == null)
                throw new Exception("Объект не принадлежит игроку, невозможно установить флаг.");

            var chip = part.Chip;
            chip.OwnerName = Name;
            _chipList.Add(chip);

            part.Chip = null;
            part.Flag = new Flag(this);
        }

        public void SetPlayerMove1(GameRoom room, string cardId, string fieldId) // положить карту
        {
            var card = room.GetCard(cardId);
            var field = room.FieldBoard.GetField(fieldId);
            LastCardId = card.Id;
            room.PutCardInField(card, field);
        }

        public void SetPlayerMove2(GameRoom room, string cardId, string partId) // положить фишку
        {
            var card = room.CardsPool.AllCards.First(_card => _card.Id == cardId);
            var partObject = card.Parts.First(_part => _part.PartId == partId);
            room.PutChipInCard(partObject, Name);
        }

        public void SetPlayerMove3(GameRoom room) // завершить ход
        {
            room.EndTurn();
        }

        public bool IsAI() => PlayerType != PlayerType.Human;

        public void ProcessMove(GameRoom room)
        {
            if (room == null)
                return;

            var card = room.GetCurrentCard();
            if (card == null)
                return; // игра уже окончена

            // where to put a card
            var fields = room.GetAvailableFields(card.Id).Select(f => f.Id).ToList();
            List<GameMove> possibleMoves = GetPossibleMoves(room.Save(), card.Id, fields);

            GameMove bestMove = GetBestMove(possibleMoves);

            // ход
            var field = room.FieldBoard.GetField(bestMove.FieldId);
            field.RotateCardTilFit(card, room.FieldBoard, room.CardsPool);
            room.PutCardInField(card, field);
            LastCardId = card.Id;
            if (bestMove.PartName != null)
            {
                var part = card.GetPart(bestMove.PartName);
                room.PutChipInCard(part, this.Name);
            }
            room.EndTurn();
        }

        private GameMove GetBestMove(List<GameMove> possibleMoves)
        {
            // ходы возвращающие фишки
            var maxReturnChips = possibleMoves.Max(m => (m.ExpectedScore.ChipCount - ChipCount));
            var returnChipsMove = possibleMoves
                .Where(m => (m.ExpectedScore.ChipCount - ChipCount) == maxReturnChips)
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
            var possibleMoves = new List<GameMove>();
            foreach (var fieldId in fieldIds)
            {
                var gameCopy = new GameRoom();
                gameCopy.Load(roomSave);
                var card = gameCopy.CardsPool.GetCard(cardId);
                var field = gameCopy.FieldBoard.GetField(fieldId);
                if (field.RotateCardTilFit(card, gameCopy.FieldBoard, gameCopy.CardsPool))
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
                        if (!part1.IsPartOfOwnedObject && ChipCount > 0)
                        {
                            gameCopy1.PutChipInCard(part1, Name);
                            gameCopy1.ScoreCalculator.CloseObjectsAndReturnChips(gameCopy1.PlayersPool, gameCopy1.CardsPool);
                        }

                        var gameMove1 = new GameMove(field.Id, part1.PartName, gameCopy1.GetPlayerScore(this));
                        possibleMoves.Add(gameMove1);
                    }

                    gameCopy.ScoreCalculator.CloseObjectsAndReturnChips(gameCopy.PlayersPool, gameCopy.CardsPool);
                    // ход без установки фишки
                    var gameMove = new GameMove(field.Id, null, gameCopy.GetPlayerScore(this));
                    possibleMoves.Add(gameMove);
                }


            }

            return possibleMoves;
        }
    }
}
