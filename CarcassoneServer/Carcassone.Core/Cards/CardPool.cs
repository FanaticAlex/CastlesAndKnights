using Carcassone.Core.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Carcassone.Core.Cards
{
    public class CardPool
    {
        private List<Card> AllCards = new List<Card>();

        /// <summary>
        /// Конструктор. Создает колоду карт.
        /// </summary>
        public CardPool()
        {
            // речные карты кладем их поверх остальных в колоде
            var riverCards = new List<Card>();
            AddCardToPool(typeof(FFWF), 1, riverCards); // начало реки

            var middleRiverCards = new List<Card>();
            AddCardToPool(typeof(CWCW), 1, middleRiverCards);
            AddCardToPool(typeof(FWRW), 1, middleRiverCards);
            AddCardToPool(typeof(FWWF), 1, middleRiverCards);
            AddCardToPool(typeof(FWWF_1), 1, middleRiverCards);
            AddCardToPool(typeof(RRWW), 1, middleRiverCards);
            AddCardToPool(typeof(RWRW), 1, middleRiverCards);
            AddCardToPool(typeof(WCCW), 1, middleRiverCards);
            AddCardToPool(typeof(WCWR), 1, middleRiverCards);
            AddCardToPool(typeof(WFWF), 1, middleRiverCards);
            AddCardToPool(typeof(WFWF_1), 1, middleRiverCards);
            // тасуем крты которые не начало и не конец реки
            Shaffle(middleRiverCards);
            riverCards.AddRange(middleRiverCards);

            AddCardToPool(typeof(WFFF), 1, riverCards); // окончание реки
            AllCards.AddRange(riverCards);

            // границы карт нумеруются с севера по часовой стрелке.
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

            // тасуем колоду
            Shaffle(baseCards);

            AllCards.AddRange(baseCards);
        }

        public Card GetCard(string cardName)
        {
            var card = AllCards.FirstOrDefault(card => card.CardName == cardName);
            if (card == null)
                throw new Exception($"Card name: '{cardName}' does not exist in the cardPool.");

            return card;
        }

        public List<Card> GetAllCards() => AllCards;

        public List<Card> GetCardsRemainInPool()
        {
            return AllCards.Where(c => c.Field == null).ToList();
        }

        /// <summary>
        /// Return card from top if the card pool
        /// </summary>
        /// <param name="fieldBoard"></param>
        /// <returns></returns>
        public Card? GetCurrentCard(FieldBoard fieldBoard)
        {
            List<Field> fields = fieldBoard.GetAvailableFields();
            var cardsRemainInPool = GetCardsRemainInPool();
            foreach (var card in cardsRemainInPool)
            {
                // проверяем можно ли эту карту сыграть, если нет берем следующую
                foreach (var field in fields)
                {
                    if (field.CanPutCardInThisFieldWithRotation(card))
                        return card;
                }
            }

            return null;
        }

        private void Shaffle(List<Card> cardsPile)
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

        private void AddCardToPool(Type cardType, byte count, List<Card> cardsPile)
        {
            for (int i = 0; i < count; i++)
            {
                var name = cardType.Name.Replace(cardType.Namespace ?? string.Empty, string.Empty);
                var card = (Card?)Activator.CreateInstance(cardType, name + "_" + i);
                if (card == null)
                    throw new Exception($"Невозможно создать карту типа {cardType} / {name + "_" + i}");

                cardsPile.Add(card);
            }
        }
    }
}
