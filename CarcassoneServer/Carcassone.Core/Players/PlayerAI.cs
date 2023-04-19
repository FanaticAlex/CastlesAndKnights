using System;
using System.Linq;

namespace Carcassone.Core.Players.AI
{
    public class PlayerAI : BasePlayer
    {
        private readonly Random _random = new Random();

        public PlayerAI(string name, string color, int chipCount)
            : base(name, color, chipCount)
        { }

        public void ProcessMove(GameRoom room)
        {
            if (room == null)
                return;

            var card = room.GetCurrentCard();
            if (card == null)
                return; // игра уже окончена

            // where to put a card
            var fields = room.GetAvailableFields(card.CardId);
            
            
            var field = fields[_random.Next(fields.Count)];
            if (field.RotateCardTilFit(card, room.FieldBoard, room.CardsPool))
            {
                LastCardId = card.CardId;
                room.PutCardInField(card, field);
            }

            // where to put a chip
            var parts = room.GetAvailableParts(card.CardId);
            if (parts.Count > 0)
            {
                var part1 = parts[_random.Next(parts.Count)];
                var part = card.Parts.Single(p => p.PartId == part1.PartId);
                if (!part.IsPartOfOwnedObject && ChipCount > 0)
                {
                    room.PutChipInCard(part, Name);
                }
            }

            var resultScore = room.GetPlayerScore(this);

            room.EndTurn();
        }
    }
}
