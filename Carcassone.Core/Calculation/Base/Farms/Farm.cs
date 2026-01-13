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

        public bool IsPlayerOwner(GamePlayer player, TileStack cardPool)
        {
            return GetOwnersNames(cardPool).Contains(player.Name);
        }

        public void AddPart(ObjectPart part, TileStack cardPool)
        {
            PartsIds.Add(part.PartId);
            OpenBorders.AddRange(part.Borders);
            RecalculatePartsOwner(cardPool);
        }

        public void RecalculatePartsOwner(TileStack cardPool)
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
                var adjacentFarmBorders = OpenBorders.FindAll(border2 => TileBorder.Equial(partBorder, border2));
                foreach (var adjacentFarmBorder in adjacentFarmBorders)
                {
                    // части поля одной карты не могут быть смежными.
                    // если это граница от той же карты то присоединять нельзя
                    if (partBorder.TileId == adjacentFarmBorder.TileId)
                        continue;

                    // если сторон нет тоесть граница карты не разделена дорогой или рекой,
                    // то считаем границы смежными и соединяем
                    if (adjacentFarmBorder.FarmSide == null && partBorder.FarmSide == null)
                        return true;

                    // одна граница с дорогой или рекой а другая нет, поля соеденены неверно
                    if (adjacentFarmBorder.FarmSide == null || partBorder.FarmSide == null)
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
                    if (group1.Contains(adjacentFarmBorder.FarmSide.Value) && group2.Contains(partBorder.FarmSide.Value))
                        return true;

                    if (group2.Contains(adjacentFarmBorder.FarmSide.Value) && group1.Contains(partBorder.FarmSide.Value))
                        return true;
                }
            }

            return false;
        }

        public int GetPoints(List<City> allCitys, TileStack cardPool)
        {
            // считаем все подключенные карты,
            // за каждый подключенный и законченный замок 3 очка
            // за незаконченный очков нет

            var connectedCitys = new List<City>();
            var parts = PartsIds.Select(id => cardPool.GetPart(id));
            foreach (var FarmPart in parts)
            {
                var card = cardPool.GetCard(FarmPart.CardId);
                var connectedCityParts = card.GetConnectedCityParts((FieldPart)FarmPart);

                foreach (var City in allCitys)
                {
                    var CityIsConnected = false;
                    foreach (var connectedCityPart in connectedCityParts)
                    {
                        if (City.PartsIds.Contains(connectedCityPart))
                            CityIsConnected = true;
                    }

                    if (CityIsConnected)
                        if (!connectedCitys.Contains(City))
                            connectedCitys.Add(City);
                }
            }

            var finishedCitys = connectedCitys.Where(City => City.IsFinished);
            return 3 * finishedCitys.Count();
        }


        private List<string> GetOwnersNames(TileStack cardPool)
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
