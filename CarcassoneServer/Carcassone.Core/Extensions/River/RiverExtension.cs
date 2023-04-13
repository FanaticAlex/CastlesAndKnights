using Carcassone.Core.Cards;
using Carcassone.Core.Extensions.River.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carcassone.Core.Extensions.River
{
    public class RiverExtension
    {
        public List<Card> GetCards()
        {
            var riverCards = new List<Card>();
            CardPool.AddCardToPool(typeof(FFWF), 1, riverCards); // начало реки

            var middleRiverCards = new List<Card>();
            CardPool.AddCardToPool(typeof(CWCW), 1, middleRiverCards);
            CardPool.AddCardToPool(typeof(FWRW), 1, middleRiverCards);
            CardPool.AddCardToPool(typeof(FWWF), 1, middleRiverCards);
            CardPool.AddCardToPool(typeof(FWWF_1), 1, middleRiverCards);
            CardPool.AddCardToPool(typeof(RRWW), 1, middleRiverCards);
            CardPool.AddCardToPool(typeof(RWRW), 1, middleRiverCards);
            CardPool.AddCardToPool(typeof(WCCW), 1, middleRiverCards);
            CardPool.AddCardToPool(typeof(WCWR), 1, middleRiverCards);
            CardPool.AddCardToPool(typeof(WFWF), 1, middleRiverCards);
            CardPool.AddCardToPool(typeof(WFWF_1), 1, middleRiverCards);
            // тасуем крты которые не начало и не конец реки
            CardPool.Shaffle(middleRiverCards);
            riverCards.AddRange(middleRiverCards);

            CardPool.AddCardToPool(typeof(WFFF), 1, riverCards); // окончание реки
            return riverCards;
        }
    }
}
