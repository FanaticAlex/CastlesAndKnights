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
        public bool IsFinished => (ChurchCardIds.Count == 9);
        public string BaseChurchPartId { get; set; }

        [JsonProperty(ItemConverterType = typeof(CardConverter))]
        public List<string> ChurchCardIds { get; set; } = new List<string>();

        public Church()
        {
        }

        public Church(ChurchPart basePart, FieldBoard fieldBoard)
        {
            BaseChurchPartId = basePart.PartId;

            // проходим по соседним полям если там есть карты подключаем их
            var fields = new List<Field?>();

            var centralField = fieldBoard.GetField(basePart.ChurchFieldId);
            fields.Add(centralField);
            fields.Add(fieldBoard?.GetNeighbour(centralField, FieldSide.left));
            fields.Add(fieldBoard?.GetNeighbour(centralField, FieldSide.right));

            var topField = fieldBoard?.GetNeighbour(centralField, FieldSide.top);
            fields.Add(topField);
            fields.Add(fieldBoard?.GetNeighbour(topField, FieldSide.left));
            fields.Add(fieldBoard?.GetNeighbour(topField, FieldSide.right));

            var bottomField = fieldBoard?.GetNeighbour(centralField, FieldSide.bottom);
            fields.Add(bottomField);
            fields.Add(fieldBoard?.GetNeighbour(bottomField, FieldSide.left));
            fields.Add(fieldBoard?.GetNeighbour(bottomField, FieldSide.right));

            foreach(var field in fields)
            {
                if (field?.CardName != null)
                    ChurchCardIds.Add(field.CardName);
            }
        }

        public string GetOwnerName(CardPool cardPool)
        {
            var baseChurchPart = cardPool.GetPart(BaseChurchPartId);
            return baseChurchPart.Chip?.OwnerName ?? baseChurchPart.Flag?.OwnerName;
        }

        public bool IsPlayerOwner(GamePlayer player, CardPool cardPool)
        {
            return GetOwnerName(cardPool) == player.Name;
        }

        /// <summary>
        /// Если добавляемая карта граничит с собором то добавляемм ее к этому собору
        /// </summary>
        /// <param name="card"></param>
        public void TryAddCard(Card card, Field field, FieldBoard fieldBoard, CardPool cardPool)
        {
            var baseChurchPart = (ChurchPart)cardPool.GetPart(BaseChurchPartId);
            var baseField = fieldBoard.GetField(baseChurchPart.ChurchFieldId);

            var distanceX = Math.Abs(field.X - baseField.X);
            var distanceY = Math.Abs(field.Y - baseField.Y);

            if (distanceX <= 1 && distanceY <= 1)
            {
                ChurchCardIds.Add(card.Id);
            }
        }

        public void TryRemoveCard(Card card)
        {
            if (ChurchCardIds.Contains(card.Id))
                ChurchCardIds.Remove(card.Id);
        }

        /// <summary>
        /// закрыть церковь если в ней 9 карт и вернуть фишку
        /// </summary>
        public void TryToClose(GamePlayersPool playersPool, CardPool cardPool)
        {
            if (IsFinished)
            {
                var baseChurchPart = (ChurchPart)cardPool.GetPart(BaseChurchPartId);
                if (baseChurchPart.Chip?.OwnerName != null)
                {
                    var player = playersPool.GetPlayer(baseChurchPart.Chip.OwnerName);
                    player.ReturnChipAndSetFlag(baseChurchPart);
                }
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
                return ChurchCardIds.Count * 2;
            }
            else
            {
                return ChurchCardIds.Count;
            }
        }
    }
}
