using System;
using System.Collections.Generic;
using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using Carcassone.Core.Players;
using Newtonsoft.Json;

namespace Carcassone.Core.Calculation.Objects
{
    public class Church : IClosingObject
    {
        public bool IsFinished => (ChurchCards.Count == 9);
        public ChurchPart BaseChurchPart { get; set; }

        [JsonProperty(ItemConverterType = typeof(CardConverter))]
        public List<string> ChurchCards { get; set; } = new List<string>();

        public Church()
        {
        }

        public Church(ChurchPart basePart, FieldBoard fieldBoard)
        {
            BaseChurchPart = basePart;

            // проходим по соседним полям если там есть карты подключаем их
            var fields = new List<Field?>();

            var centralField = fieldBoard.GetField(basePart.ChurchFieldId);
            fields.Add(centralField);
            fields.Add(fieldBoard?.GetNeighbour(centralField, Side.left));
            fields.Add(fieldBoard?.GetNeighbour(centralField, Side.right));

            var topField = fieldBoard?.GetNeighbour(centralField, Side.top);
            fields.Add(topField);
            fields.Add(fieldBoard?.GetNeighbour(topField, Side.left));
            fields.Add(fieldBoard?.GetNeighbour(topField, Side.right));

            var bottomField = fieldBoard?.GetNeighbour(centralField, Side.bottom);
            fields.Add(bottomField);
            fields.Add(fieldBoard?.GetNeighbour(bottomField, Side.left));
            fields.Add(fieldBoard?.GetNeighbour(bottomField, Side.right));

            foreach(var field in fields)
            {
                if (field?.CardName != null)
                    ChurchCards.Add(field.CardName);
            }
        }

        public string GetOwnerName()
        {
            return BaseChurchPart.Chip?.OwnerName ?? BaseChurchPart.Flag?.OwnerName;
        }

        public bool IsPlayerOwner(BasePlayer player)
        {
            return GetOwnerName() == player.Name;
        }

        /// <summary>
        /// Если добавляемая карта граничит с собором то добавляемм ее к этому собору
        /// </summary>
        /// <param name="card"></param>
        public void TryAddCard(Card card, Field field, FieldBoard fieldBoard)
        {
            var baseField = fieldBoard.GetField(BaseChurchPart.ChurchFieldId);

            var distanceX = Math.Abs(field.X - baseField.X);
            var distanceY = Math.Abs(field.Y - baseField.Y);

            if (distanceX <= 1 && distanceY <= 1)
            {
                ChurchCards.Add(card.CardId);
            }
        }

        public void TryRemoveCard(Card card)
        {
            if (ChurchCards.Contains(card.CardId))
                ChurchCards.Remove(card.CardId);
        }

        /// <summary>
        /// закрыть церковь если в ней 9 карт и вернуть фишку
        /// </summary>
        public void TryToClose(PlayersPool playersPool)
        {
            if (IsFinished)
            {
                var player = playersPool.GetPlayer(BaseChurchPart.Chip?.OwnerName);
                player?.ReturnChipAndSetFlag(BaseChurchPart);
            }
        }

        /// <summary>
        /// 1 карта = 1 очко. если завершено 1 карта = 2 очка.
        /// </summary>
        /// <returns></returns>
        public int GetPoints()
        {
            if (IsFinished)
            {
                return ChurchCards.Count * 2;
            }
            else
            {
                return ChurchCards.Count;
            }
        }
    }
}
