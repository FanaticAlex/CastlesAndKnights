using Carcassone.Core.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Cards
{
    public class CardPool
    {
        public CardPool(ExtensionsManager extensionsManager)
        {
            // речные карты кладем их поверх остальных в колоде
            if (extensionsManager.RiverExtension != null)
            {
                var riverCards = extensionsManager.RiverExtension.GetCards();
                AllCards.AddRange(riverCards);
            }

            var baseCards = new List<Card>();
            AddCardToPool(typeof(CCCC), 1, baseCards);
            AddCardToPool(typeof(CCFC), 3, baseCards);
            AddCardToPool(typeof(CCFC_1), 1, baseCards);
            AddCardToPool(typeof(CCFF), 2, baseCards);
            AddCardToPool(typeof(CCRC), 1, baseCards);
            AddCardToPool(typeof(CCRC_1), 2, baseCards);
            AddCardToPool(typeof(CFFC), 3, baseCards);
            AddCardToPool(typeof(CFFC_1), 2, baseCards);
            AddCardToPool(typeof(CFFF), 5, baseCards);
            AddCardToPool(typeof(CFRR), 3, baseCards);
            AddCardToPool(typeof(CRFR), 3, baseCards);
            AddCardToPool(typeof(CRRC), 3, baseCards);
            AddCardToPool(typeof(CRRC_1), 2, baseCards);
            AddCardToPool(typeof(CRRF), 3, baseCards);
            AddCardToPool(typeof(CRRR), 3, baseCards);

            AddCardToPool(typeof(FCFC), 3, baseCards);
            AddCardToPool(typeof(FCFC_1), 1, baseCards);
            AddCardToPool(typeof(FCFC_2), 2, baseCards);
            AddCardToPool(typeof(FFFF), 4, baseCards);
            AddCardToPool(typeof(FFRF), 2, baseCards);
            AddCardToPool(typeof(FFRR), 8, baseCards);
            AddCardToPool(typeof(FRRR), 4, baseCards);

            AddCardToPool(typeof(RFRF), 8, baseCards);
            AddCardToPool(typeof(RRRR), 1, baseCards);

            Shaffle(baseCards);

            AllCards.AddRange(baseCards);
        }

        [JsonProperty(ItemConverterType = typeof(CardConverter), ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public List<Card> AllCards { get; private set; } = new List<Card>();

        public Card GetCard(string cardId) => AllCards.FirstOrDefault(card => card.Id == cardId);

        public ObjectPart GetPart(string partId)
        {
            return AllCards.SelectMany(c => c.Parts).FirstOrDefault(p => p.PartId == partId);
        }

        public static void Shaffle(List<Card> cardsPile)
        {
            // перетосовка
            var rnd = new System.Random();
            int n = cardsPile.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                var value = cardsPile[k];
                cardsPile[k] = cardsPile[n];
                cardsPile[n] = value;
            }
        }

        public static void AddCardToPool(Type cardType, byte count, List<Card> cardsPile)
        {
            for (int i = 0; i < count; i++)
            {
                var cardTypeStr = cardType.Name.Replace(cardType.Namespace ?? string.Empty, string.Empty);
                var card = (Card?)Activator.CreateInstance(cardType, cardTypeStr, i);
                if (card == null)
                    throw new Exception($"Can't create card of type {cardTypeStr}: {i}");

                cardsPile.Add(card);
            }
        }
    }
}
