using Carcassone.Core.Cards;
using Carcassone.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Calculation.Objects
{
    public class Cornfield : IMultipartObject
    {
        public List<Border> OpenBorders = new List<Border>();

        public List<ObjectPart> Parts { get; set; } = new List<ObjectPart>();

        public List<ObjectPart> GetParts() => Parts;

        public bool IsPlayerOwner(BasePlayer player)
        {
            return GetOwners().Select(owner => owner.Name).Contains(player.Name);
        }

        public void AddPart(ObjectPart part)
        {
            Parts.Add(part);
            OpenBorders.AddRange(part.Borders);
            RecalculatePartsOwner();
        }

        public void RecalculatePartsOwner()
        {
            if (GetOwners().Count > 0)
            {
                foreach (var part1 in Parts)
                {
                    part1.IsOwned = true;
                }
            }
        }

        public bool CanConnect(ObjectPart part)
        {
            foreach (var partBorder in part.Borders)
            {
                // ищем открытые границы этого поля которые совпадают с границами присоединяемой части
                var adjacentCornfieldBorders = OpenBorders.FindAll(border2 => Border.Equial(partBorder, border2));
                foreach (var adjacentCornfieldBorder in adjacentCornfieldBorders)
                {
                    // части поля одной карты не могут быть смежными.
                    // если это граница от той же карты то присоединять нельзя
                    if (partBorder.Card == adjacentCornfieldBorder.Card)
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

                    var group1 = new List<CornfieldSide>() { CornfieldSide.side_0, CornfieldSide.side_2, CornfieldSide.side_4, CornfieldSide.side_6 };
                    var group2 = new List<CornfieldSide>() { CornfieldSide.side_1, CornfieldSide.side_3, CornfieldSide.side_5, CornfieldSide.side_7 };

                    // проверяемые части должны быть в разных группах
                    if (group1.Contains(adjacentCornfieldBorder.CornfieldSide.Value) && group2.Contains(partBorder.CornfieldSide.Value))
                        return true;

                    if (group2.Contains(adjacentCornfieldBorder.CornfieldSide.Value) && group1.Contains(partBorder.CornfieldSide.Value))
                        return true;
                }
            }

            return false;
        }

        public int GetPoints(List<Castle> allCastles, GameRoom room)
        {
            // считаем все подключенные карты,
            // за каждый подключенный и законченный замок 3 очка
            // за незаконченный очков нет

            var connectedCastles = new List<Castle>();
            foreach(var cornfieldPart in Parts)
            {
                var card = room.GetCard(cornfieldPart.CardName);
                var connectedCastleParts = card.GetConnectedCastleParts((CornfieldPart)cornfieldPart);

                foreach (var castle in allCastles)
                {
                    var castleIsConnected = false;
                    foreach (var connectedCastlePart in connectedCastleParts)
                    {
                        if (castle.Parts.Contains(connectedCastlePart))
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


        private List<BasePlayer> GetOwners()
        {
            var owners = new List<BasePlayer>();
            foreach (var part in Parts)
            {
                if (part.Chip != null && !owners.Contains(part.Chip.Owner))
                {
                    owners.Add(part.Chip.Owner);
                }
            }

            return owners;
        }
    }
}
