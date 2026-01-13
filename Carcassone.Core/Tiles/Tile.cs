using System;
using System.Collections.Generic;
using System.Linq;
using Carcassone.Core.Board;
using Carcassone.Core.Calculation;
using Carcassone.Core.Calculation.Base.Farms;
using Newtonsoft.Json;

namespace Carcassone.Core.Tiles
{
    /// <summary>
    /// Game tile. Square card represents landscape or parts of game objects.
    /// Could be placed and connected with other tiles.
    /// </summary>
    public abstract class Tile
    {
        /// <summary>
        /// Вручную соединенные замки и поля,
        /// это нужно для подсчета какие замки присоденены к полям при подсчете очков за поля
        /// </summary>
        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public Dictionary<string, List<string>> FieldToCityParts { get; set; } = new Dictionary<string, List<string>>();

        [JsonProperty(ItemConverterType = typeof(PartConverter), ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public List<ObjectPart> Parts { get; set; } = new List<ObjectPart>();

        public string Id { get; set; }
        public string CardType { get; set; }
        public int CardNumber { get; set; }

        /// <summary>
        /// define where in a tile stack pile this card should apear
        /// </summary>
        public int StackPriority { get; set; }

        public char TopEdgeType { get; set; }
        public char RightEdgeType { get; set; }
        public char BottomEdgeType { get; set; }
        public char LeftEdgeType { get; set; }


        public int RotationsCount { get; set; }

        public Tile(string cardType, int cardNumber)
        {
            Id = GetTileId(cardType, cardNumber);
            CardType = cardType;
            CardNumber = cardNumber;
            TopEdgeType = cardType[0];
            RightEdgeType = cardType[1];
            BottomEdgeType = cardType[2];
            LeftEdgeType = cardType[3];
        }

        public static string GetTileId(string cardType, int cardNumber) => $"{cardType}({cardNumber})";

        public ObjectPart GetPart(string partName)
        {
            var list = Parts.Where(p => p.PartName == partName);
            if (list.Count() == 0) throw new NullReferenceException("Part not found: " + partName);

            if (list.Count() > 1) throw new InvalidOperationException("Не может быть два обьекта с одним названием");

            return list.Single();
        }

        public void AddBorderToPart(Cell cell, CellSide side, ObjectPart? part, Grid grid)
        {
            if (part == null) throw new ArgumentNullException(nameof(part));

            var rotatedSide = GetRotatedSide(side, RotationsCount);
            var border = new TileBorder(cell, grid.GetNeighbour(cell, rotatedSide), this);
            part.Borders.Add(border);
        }

        public void AddFarmSplittedBorder(
            Cell field, CellSide side, FieldSide sidePart, ObjectPart? part, Grid grid)
        {
            if (part == null) throw new ArgumentNullException(nameof(part));

            if ((side == CellSide.top && sidePart != FieldSide.side_0 && sidePart != FieldSide.side_7) ||
                (side == CellSide.right && sidePart != FieldSide.side_1 && sidePart != FieldSide.side_2) ||
                (side == CellSide.bottom && sidePart != FieldSide.side_3 && sidePart != FieldSide.side_4) ||
                (side == CellSide.left && sidePart != FieldSide.side_5 && sidePart != FieldSide.side_6))
            {
                throw new Exception($"Wrong side order in part {part.PartId}");
            }

            side = GetRotatedSide(side, RotationsCount);
            sidePart = GetRotatedSidePart(sidePart, RotationsCount);
            var Farm1Border0 = new TileBorder(field, grid.GetNeighbour(field, side), this);
            Farm1Border0.FarmSide = sidePart;
            part.Borders.Add(Farm1Border0);
        }

        public List<string> GetConnectedCityParts(FieldPart part)
        {
            var CityParts = new List<string>();
            if (FieldToCityParts.Keys.Contains(part.PartId))
            {
                CityParts = FieldToCityParts[part.PartId];
            }

            return CityParts;
        }

        public abstract void ConnectField(Cell field, Grid grid);

        public void RotateCard(int rotation)
        {
            if (rotation < 0) throw new ArgumentOutOfRangeException("неверное число поворотов");
            if (rotation >= 4) throw new ArgumentOutOfRangeException("неверное число поворотов");

            do
                RotateCard();
            while (RotationsCount != rotation);
        }

        /// <summary>
        /// Поворачивает карту на 90 по часовой стрелке градусов счетчик поворотов увеличивается на 1
        /// </summary>
        public void RotateCard()
        {
            var tempTop = TopEdgeType;
            var tempLeft = LeftEdgeType;
            var tempBottom = BottomEdgeType;
            var tempRight = RightEdgeType;

            TopEdgeType = tempLeft;
            LeftEdgeType = tempBottom;
            BottomEdgeType = tempRight;
            RightEdgeType = tempTop;

            RotationsCount += 1;
            RotationsCount %= 4;
        }

        /// <summary>
        /// Трансформация исходной стороны с учетом поворота карты
        /// </summary>
        /// <param name="side"></param>
        /// <param name="rotationCount"></param>
        /// <returns></returns>
        private static CellSide GetRotatedSide(CellSide side, int rotationCount)
        {
            var result = ((byte)side + rotationCount) % 4;
            return (CellSide)result;
        }

        /// <summary>
        /// Поворот стороны
        /// </summary>
        /// <param name="side"></param>
        /// <param name="rotationCount"></param>
        /// <returns></returns>
        private static FieldSide GetRotatedSidePart(FieldSide side, int rotationCount)
        {
            var result = ((byte)side + rotationCount * 2) % 8;
            return (FieldSide)result;
        }
    }
}