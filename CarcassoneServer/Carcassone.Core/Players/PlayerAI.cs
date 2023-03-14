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
            var fields = room.GetAvailableFields(card.CardName);
            var field = fields[_random.Next(fields.Count)];
            if (field.RotateCardTilFit(card))
            {
                LastCardId = card.CardName;
                room.PutCardInField(card, field);
            }

            // where to put a chip
            var parts = room.GetAvailableParts(card.CardName);
            if (parts.Count > 0)
            {
                var part1 = parts[_random.Next(parts.Count)];
                var part = card.Parts.Single(p => p.PartId == part1.PartId);
                if (!part.IsOwned && ChipCount > 0)
                {
                    room.PutChipInCard(part, this);
                }
            }

            room.EndTurn();
        }
    }
}
