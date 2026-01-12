using Carcassone.Core.Calculation;
using Carcassone.Core.Calculation.Base.Tiles;
using Carcassone.Core.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Tiles
{
    /// <summary>
    /// Stack of tiles in a current game
    /// </summary>
    public class Stack
    {
        private readonly List<Tile> _remainTiles = new List<Tile>();
        private readonly List<Tile> _discardedTiles = new List<Tile>();

        [JsonConverter(typeof(TileConverter))]
        public Tile? CurrentCard { get; set; }

        public Stack(ExtensionsManager extensionsManager)
        {
            // add river cards first
            if (extensionsManager.RiverExtension != null)
            {
                var riverCards = extensionsManager.RiverExtension.GetCards();
                _remainTiles.AddRange(riverCards);
            }

            var baseTiles = new List<Tile>();
            AddCardToPool(typeof(CCCC), 1, baseTiles);
            AddCardToPool(typeof(CCFC), 3, baseTiles);
            AddCardToPool(typeof(CCFC_1), 1, baseTiles);
            AddCardToPool(typeof(CCFF), 2, baseTiles);
            AddCardToPool(typeof(CCRC), 1, baseTiles);
            AddCardToPool(typeof(CCRC_1), 2, baseTiles);
            AddCardToPool(typeof(CFFC), 3, baseTiles);
            AddCardToPool(typeof(CFFC_1), 2, baseTiles);
            AddCardToPool(typeof(CFFF), 5, baseTiles);
            AddCardToPool(typeof(CFRR), 3, baseTiles);
            AddCardToPool(typeof(CRFR), 3, baseTiles);
            AddCardToPool(typeof(CRRC), 3, baseTiles);
            AddCardToPool(typeof(CRRC_1), 2, baseTiles);
            AddCardToPool(typeof(CRRF), 3, baseTiles);
            AddCardToPool(typeof(CRRR), 3, baseTiles);

            AddCardToPool(typeof(FCFC), 3, baseTiles);
            AddCardToPool(typeof(FCFC_1), 1, baseTiles);
            AddCardToPool(typeof(FCFC_2), 2, baseTiles);
            AddCardToPool(typeof(FFFF), 4, baseTiles);
            AddCardToPool(typeof(FFRF), 2, baseTiles);
            AddCardToPool(typeof(FFRR), 8, baseTiles);
            AddCardToPool(typeof(FRRR), 4, baseTiles);

            AddCardToPool(typeof(RFRF), 8, baseTiles);
            AddCardToPool(typeof(RRRR), 1, baseTiles);

            Shaffle(baseTiles);

            _remainTiles.AddRange(baseTiles);
        }

        public List<Tile> GetRemainTiles()
        {
            return _remainTiles;
        }

        public List<Tile> GetAllTiles()
        {
            var allCards = new List<Tile>();
            allCards.AddRange(_remainTiles);
            allCards.AddRange(_discardedTiles);
            return allCards;
        }

        public Tile GetCard(string cardId)
        {
            var card = GetAllTiles().FirstOrDefault(card => card.Id == cardId);
            if (card == null)
                throw new Exception($"Card {cardId} not found");

            return card;
        }

        public bool IsEmpty()
        {
            return _remainTiles.Count == 0;
        }

        public Tile? GetTopCard()
        {
            return _remainTiles.FirstOrDefault();
        }

        public void DiscardCard(Tile? card)
        {
            if (card == null) return;

            _remainTiles.Remove(card);
            _discardedTiles.Add(card);
        }

        public ObjectPart GetPart(string partId)
        {
            return GetAllTiles().SelectMany(c => c.Parts).FirstOrDefault(p => p.PartId == partId);
        }

        public static void Shaffle(List<Tile> cardsPile)
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

        public static void AddCardToPool(Type cardType, byte count, List<Tile> cardsPile)
        {
            for (int i = 0; i < count; i++)
            {
                var cardTypeStr = cardType.Name.Replace(cardType.Namespace ?? string.Empty, string.Empty);
                var card = (Tile?)Activator.CreateInstance(cardType, cardTypeStr, i);
                if (card == null)
                    throw new Exception($"Can't create card of type {cardTypeStr}: {i}");

                cardsPile.Add(card);
            }
        }
    }
}
