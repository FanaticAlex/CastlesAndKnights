using Carcassone.Core.Tiles;
using System.Collections.Generic;
using Carcassone.Core.Calculation.River.Tiles;

namespace Carcassone.Core.Calculation.River
{
    /// <summary>
    /// This extension is adding river cards to the game.
    /// </summary>
    public class RiverExtension
    {
        public List<Tile> GetCards()
        {
            var riverCards = new List<Tile>();
            Stack.AddCardToPool(typeof(FFWF), 1, riverCards); // начало реки

            var middleRiverCards = new List<Tile>();
            Stack.AddCardToPool(typeof(CWCW), 1, middleRiverCards);
            Stack.AddCardToPool(typeof(FWRW), 1, middleRiverCards);
            Stack.AddCardToPool(typeof(FWWF), 1, middleRiverCards);
            Stack.AddCardToPool(typeof(FWWF_1), 1, middleRiverCards);
            Stack.AddCardToPool(typeof(RRWW), 1, middleRiverCards);
            Stack.AddCardToPool(typeof(RWRW), 1, middleRiverCards);
            Stack.AddCardToPool(typeof(WCCW), 1, middleRiverCards);
            Stack.AddCardToPool(typeof(WCWR), 1, middleRiverCards);
            Stack.AddCardToPool(typeof(WFWF), 1, middleRiverCards);
            Stack.AddCardToPool(typeof(WFWF_1), 1, middleRiverCards);
            // тасуем крты которые не начало и не конец реки
            Stack.Shaffle(middleRiverCards);
            riverCards.AddRange(middleRiverCards);

            Stack.AddCardToPool(typeof(WFFF), 1, riverCards); // окончание реки
            return riverCards;
        }
    }
}
