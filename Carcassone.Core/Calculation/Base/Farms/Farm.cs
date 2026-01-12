using Carcassone.Core.Tiles;
using Carcassone.Core.Players;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Carcassone.Core.Calculation.Base.Cities;

namespace Carcassone.Core.Calculation.Base.Farms
{
    public class Farm : IMultipartObject, IOwnedObject
    {
        public List<TileBorder> OpenBorders = new List<TileBorder>();

        public List<string> PartsIds { get; set; } = new List<string>();

        public bool IsPlayerOwner(GamePlayer player, Stack cardPool)
        {
            return GetOwnersNames(cardPool).Contains(player.Name);
        }

        public void AddPart(ObjectPart part, Stack cardPool)
        {
            PartsIds.Add(part.PartId);
            OpenBorders.AddRange(part.Borders);
            RecalculatePartsOwner(cardPool);
        }

        public void RecalculatePartsOwner(Stack cardPool)
        {
            if (GetOwnersNames(cardPool).Count > 0)
            {
                var parts = PartsIds.Select(id => cardPool.GetPart(id));
                foreach (var part1 in parts)
                {
                    part1.IsPartOfOwnedObject = true;
                }
            }
        }

        public bool CanConnect(ObjectPart part)
        {
            foreach (var partBorder in part.Borders)
            {
                // ищем открытые границы этого поля которые совпадают с границами присоединяемой части
                var adjacentCornfieldBorders = OpenBorders.FindAll(border2 => TileBorder.Equial(partBorder, border2));
                foreach (var adjacentCornfieldBorder in adjacentCornfieldBorders)
                {
                    // части поля одной карты не могут быть смежными.
                    // если это граница от той же карты то присоединять нельзя
                    if (partBorder.TileId == adjacentCornfieldBorder.TileId)
                        continue;

                    // если сторон нет тоесть граница карты не разделена дорогой или рекой,
                    // то считаем границы смежными и соединяем
                    if (adjacentCornfieldBorder.CornfieldSide == null && partBorder.CornfieldSide == null)
                        return true;

                    // одна граница с дорогой или рекой а другая нет, поля соеденены неверно
                    if (adjacentCornfieldBorder.CornfieldSide == null || partBorder.CornfieldSide == null)
                        throw new Exception("поля соеденены неверно");

                    // проверяем четность. 
                    // Группа на одной стороне разделителя, карты могут быть повернуты
                    //     7  0      |      7  0
                    // 6 |      | 1  |  6 |      | 1
                    //   |      |    |    |      |
                    // 5 |      | 2  |  5 |      | 2
                    //     4  3      |      4  3

                    var group1 = new List<FieldSide>() { FieldSide.side_0, FieldSide.side_2, FieldSide.side_4, FieldSide.side_6 };
                    var group2 = new List<FieldSide>() { FieldSide.side_1, FieldSide.side_3, FieldSide.side_5, FieldSide.side_7 };

                    // проверяемые части должны быть в разных группах
                    if (group1.Contains(adjacentCornfieldBorder.CornfieldSide.Value) && group2.Contains(partBorder.CornfieldSide.Value))
                        return true;

                    if (group2.Contains(adjacentCornfieldBorder.CornfieldSide.Value) && group1.Contains(partBorder.CornfieldSide.Value))
                        return true;
                }
            }

            return false;
        }

        public int GetPoints(List<City> allCastles, Stack cardPool)
        {
            // считаем все подключенные карты,
            // за каждый подключенный и законченный замок 3 очка
            // за незаконченный очков нет

            var connectedCastles = new List<City>();
            var parts = PartsIds.Select(id => cardPool.GetPart(id));
            foreach (var cornfieldPart in parts)
            {
                var card = cardPool.GetCard(cornfieldPart.CardId);
                var connectedCastleParts = card.GetConnectedCastleParts((FieldPart)cornfieldPart);

                foreach (var castle in allCastles)
                {
                    var castleIsConnected = false;
                    foreach (var connectedCastlePart in connectedCastleParts)
                    {
                        if (castle.PartsIds.Contains(connectedCastlePart))
                            castleIsConnected = true;
                    }

                    if (castleIsConnected)
                        if (!connectedCastles.Contains(castle))
                            connectedCastles.Add(castle);
                }
            }

            var finishedCastles = connectedCastles.Where(castle => castle.IsFinished);
            return 3 * finishedCastles.Count();
        }


        private List<string> GetOwnersNames(Stack cardPool)
        {
            var owners = new List<string>();
            var parts = PartsIds.Select(id => cardPool.GetPart(id));
            foreach (var part in parts)
            {
                if (part.Chip != null && !owners.Contains(part.Chip.OwnerName))
                {
                    owners.Add(part.Chip.OwnerName);
                }
            }

            return owners;
        }
    }
}
