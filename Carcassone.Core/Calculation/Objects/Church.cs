using System;
using System.Collections.Generic;
using Carcassone.Core.Tiles;
using Carcassone.Core.Board;
using Carcassone.Core.Players;
using Newtonsoft.Json;

namespace Carcassone.Core.Calculation.Objects
{
    public class Church : IClosingObject, IOwnedObject
    {
        public bool IsFinished => (ChurchCardIds.Count == 9);
        public string BaseChurchPartId { get; set; }

        [JsonProperty(ItemConverterType = typeof(TileConverter))]
        public List<string> ChurchCardIds { get; set; } = new List<string>();

        public Church()
        {
        }

        public Church(ChurchPart basePart, Grid grid)
        {
            BaseChurchPartId = basePart.PartId;

            // проходим по соседним полям если там есть карты подключаем их
            var fields = new List<Cell?>();

            var centralField = grid.GetField(basePart.ChurchFieldId);
            fields.Add(centralField);
            fields.Add(grid?.GetNeighbour(centralField, CellSide.left));
            fields.Add(grid?.GetNeighbour(centralField, CellSide.right));

            var topField = grid?.GetNeighbour(centralField, CellSide.top);
            fields.Add(topField);
            fields.Add(grid?.GetNeighbour(topField, CellSide.left));
            fields.Add(grid?.GetNeighbour(topField, CellSide.right));

            var bottomField = grid?.GetNeighbour(centralField, CellSide.bottom);
            fields.Add(bottomField);
            fields.Add(grid?.GetNeighbour(bottomField, CellSide.left));
            fields.Add(grid?.GetNeighbour(bottomField, CellSide.right));

            foreach(var field in fields)
            {
                if (field?.CardName != null)
                    ChurchCardIds.Add(field.CardName);
            }
        }

        public string? GetOwnerName(Stack cardPool)
        {
            var baseChurchPart = cardPool.GetPart(BaseChurchPartId);
            return baseChurchPart.Chip?.OwnerName ?? baseChurchPart.Flag?.OwnerName;
        }

        public bool IsPlayerOwner(GamePlayer player, Stack cardPool)
        {
            return GetOwnerName(cardPool) == player.Name;
        }

        /// <summary>
        /// Если добавляемая карта граничит с собором то добавляемм ее к этому собору
        /// </summary>
        /// <param name="card"></param>
        public void TryAddCard(Tile card, Cell field, Grid grid, Stack cardPool)
        {
            var baseChurchPart = (ChurchPart)cardPool.GetPart(BaseChurchPartId);
            var baseField = grid.GetField(baseChurchPart.ChurchFieldId);

            var distanceX = Math.Abs(field.X - baseField.X);
            var distanceY = Math.Abs(field.Y - baseField.Y);

            if (distanceX <= 1 && distanceY <= 1)
            {
                ChurchCardIds.Add(card.Id);
            }
        }

        public void TryRemoveCard(Tile card)
        {
            if (ChurchCardIds.Contains(card.Id))
                ChurchCardIds.Remove(card.Id);
        }

        /// <summary>
        /// закрыть церковь если в ней 9 карт и вернуть фишку
        /// </summary>
        public void TryToClose(GamePlayersPool playersPool, Stack cardPool)
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
