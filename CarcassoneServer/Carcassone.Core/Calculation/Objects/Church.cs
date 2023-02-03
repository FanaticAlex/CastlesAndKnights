using System;
using System.Collections.Generic;
using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using Carcassone.Core.Players;

namespace Carcassone.Core.Calculation.Objects
{
    public class Church : IClosingObject
    {
        public bool IsFinished => (ChurchElements.Count == 9);
        public ChurchPart BaseChurchPart { get; set; }
        public List<Card> ChurchElements { get; set; } = new List<Card>();

        public Church(ChurchPart basePart)
        {
            BaseChurchPart = basePart;

            // проходим по соседним полям если там есть карты подключаем их
            var fields = new List<Field?>();
            var centralField = basePart.ChurchField;
            fields.Add(centralField);
            fields.Add(centralField?.GetNeighbour(Side.left));
            fields.Add(centralField?.GetNeighbour(Side.right));

            var topField = centralField?.GetNeighbour(Side.top);
            fields.Add(topField);
            fields.Add(topField?.GetNeighbour(Side.left));
            fields.Add(topField?.GetNeighbour(Side.right));

            var bottomField = centralField?.GetNeighbour(Side.bottom);
            fields.Add(bottomField);
            fields.Add(bottomField?.GetNeighbour(Side.left));
            fields.Add(bottomField?.GetNeighbour(Side.right));

            foreach(var field in fields)
            {
                if (field?.GetCardInField() != null)
                    ChurchElements.Add(field.GetCardInField());
            }
        }

        public Player? Owner => BaseChurchPart.Chip?.Owner ?? BaseChurchPart.Flag?.Owner;

        public bool IsPlayerOwner(Player player)
        {
            return Owner?.Name == player.Name;
        }

        public void TryAddCard(Card card)
        {
            var distanceX = Math.Abs(card.Field.X - BaseChurchPart.ChurchField.X);
            var distanceY = Math.Abs(card.Field.Y - BaseChurchPart.ChurchField.Y);

            if (distanceX <= 1 && distanceY <= 1)
            {
                ChurchElements.Add(card);
            }
        }

        /// <summary>
        /// закрыть церковь если в ней 9 карт и вернуть фишку
        /// </summary>
        public void TryToClose()
        {
            if (IsFinished)
                BaseChurchPart.Chip?.Owner.ReturnChipAndSetFlag(BaseChurchPart);
        }

        /// <summary>
        /// 1 карта = 1 очко. если завершено 1 карта = 2 очка.
        /// </summary>
        /// <returns></returns>
        public int GetPoints()
        {
            if (IsFinished)
            {
                return ChurchElements.Count * 2;
            }
            else
            {
                return ChurchElements.Count;
            }
        }
    }
}
