using System;
using System.Collections.Generic;
using System.Linq;
using Carcassone.Core.Board;
using Carcassone.Core.Tiles.Objects;
using Newtonsoft.Json;

namespace Carcassone.Core.Tiles
{
    /// <summary>
    /// Игровая карта. определяет местность на поле.
    /// </summary>
    public abstract class Tile
    {
        /// <summary>
        /// Вручную соединенные замки и поля,
        /// это нужно для подсчета какие замки присоденены к полям при подсчете очков за поля
        /// </summary>
        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public Dictionary<string, List<string>> FieldToCastleParts { get; set; } = new Dictionary<string, List<string>>();

        [JsonProperty(ItemConverterType = typeof(PartConverter), ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public List<ObjectPart> Parts { get; set; } = new List<ObjectPart>();

        public string Id { get; set; }
        public string CardType { get; set; }
        public int CardNumber { get; set; }

        public CardEdgeType TopEdgeType { get; set; } = CardEdgeType.None;
        public CardEdgeType RightEdgeType { get; set; } = CardEdgeType.None;
        public CardEdgeType BottomEdgeType { get; set; } = CardEdgeType.None;
        public CardEdgeType LeftEdgeType { get; set; } = CardEdgeType.None;

        public CenterType CenterType = CenterType.None;

        public int RotationsCount { get; set; }

        public Tile(string cardType, int cardNumber)
        {
            Dictionary<char, CardEdgeType> nameDict = new Dictionary<char, CardEdgeType>()
            {
                { 'R', CardEdgeType.Road },
                { 'C', CardEdgeType.Castle },
                { 'F', CardEdgeType.Cornfield },
                { 'W', CardEdgeType.Water }
            };

            Id = GetCardId(cardType, cardNumber);
            CardType = cardType;
            CardNumber = cardNumber;
            TopEdgeType = nameDict[cardType[0]];
            RightEdgeType = nameDict[cardType[1]];
            BottomEdgeType = nameDict[cardType[2]];
            LeftEdgeType = nameDict[cardType[3]];
        }

        public static string GetCardId(string cardType, int cardNumber) => $"{cardType}({cardNumber})";

        public ObjectPart GetPart(string partName)
        {
            var list = Parts.Where(p => p.PartName == partName);
            if (list.Count() == 0) throw new NullReferenceException("Part not found: " + partName);

            if (list.Count() > 1) throw new InvalidOperationException("Не может быть два обьекта с одним названием");

            return list.Single();
        }

        public void AddBorderToPart(Cell field, CellSide side, ObjectPart? part, Grid grid)
        {
            if (part == null) throw new ArgumentNullException(nameof(part));

            var rotatedSide = RotateSide(side, RotationsCount);
            var castleBorder = new CardBorder(field, grid.GetNeighbour(field, rotatedSide), this);
            part.Borders.Add(castleBorder);
        }

        public void AddCornfieldSplittedBorder(
            Cell field, CellSide side, CornfieldSide sidePart, ObjectPart? part, Grid grid)
        {
            if (part == null) throw new ArgumentNullException(nameof(part));

            if ((side == CellSide.top && sidePart != CornfieldSide.side_0 && sidePart != CornfieldSide.side_7) ||
                (side == CellSide.right && sidePart != CornfieldSide.side_1 && sidePart != CornfieldSide.side_2) ||
                (side == CellSide.bottom && sidePart != CornfieldSide.side_3 && sidePart != CornfieldSide.side_4) ||
                (side == CellSide.left && sidePart != CornfieldSide.side_5 && sidePart != CornfieldSide.side_6))
            {
                throw new Exception($"Wrong side order in part {part.PartId}");
            }

            side = RotateSide(side, RotationsCount);
            sidePart = RotateSidePart(sidePart, RotationsCount);
            var cornfield1Border0 = new CardBorder(field, grid.GetNeighbour(field, side), this);
            cornfield1Border0.CornfieldSide = sidePart;
            part.Borders.Add(cornfield1Border0);
        }

        public List<string> GetConnectedCastleParts(FieldPart part)
        {
            var castleParts = new List<string>();
            if (FieldToCastleParts.Keys.Contains(part.PartId))
            {
                castleParts = FieldToCastleParts[part.PartId];
            }

            return castleParts;
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
        public static CellSide RotateSide(CellSide side, int rotationCount)
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
        public static CornfieldSide RotateSidePart(CornfieldSide side, int rotationCount)
        {
            var result = ((byte)side + rotationCount * 2) % 8;
            return (CornfieldSide)result;
        }
    }
}