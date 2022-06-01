using Carcassone.Core.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carcassone.Core.Players.AI
{
    internal class PlayerAI
    {
        private Random _random = new Random();

        public void MakeMove(GameRoom room, Player player)
        {
            var card = room.GetCurrentCard();
            if (card == null)
                return; // игра уже окончена

            var fields = room.GetAvailableFields(card.CardName);
            var field = fields[_random.Next(fields.Count)];
            if (field.RotateCardTilFit(card))
            {
                room.PutCardInField(card, field);
            }

            var parts = room.GetAvailableParts(card.CardName);
            if (parts.Count > 0)
            {
                var part1 = parts[_random.Next(parts.Count)];
                var part = card.Parts.Single(p => p.PartId == part1.PartId);
                if (!part.IsOwned && player.ChipCount > 0)
                {
                    room.PutChipInCard(card.CardName, part.PartId, player.Name);
                }
            }
        }
    }
}
