using System.Collections.Generic;
using System.Linq;
using Carcassone.Core.Fields;
using Newtonsoft.Json;

namespace Carcassone.Core.Cards
{
    /// <summary>
    /// Игровая карта. определяет местность на поле.
    /// </summary>
    public abstract class Card
    {
        /// <summary>
        /// Вручную соединенные замки и поля,
        /// это нужно для подсчета какие замки присоденены к полямм при подсчете очков за поля
        /// </summary>
        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public Dictionary<string, List<string>> FieldToCastleParts { get; set; } =
            new Dictionary<string, List<string>>();

        /// <summary>
        /// Части обьектов на этой карте
        /// </summary>
        [JsonProperty(ItemConverterType = typeof(PartConverter), ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public List<ObjectPart> Parts { get; set; } = new List<ObjectPart>();

        /// <summary>
        /// Полное название карты (CardType+(CardNumber)), например СССС_1(0).
        /// </summary>
        public string CardId { get; set; }

        public string CardType { get; set; }

        public int CardNumber { get; set; }

        public EdgeType TopEdgeType { get; set; } = EdgeType.None;
        public EdgeType RightEdgeType { get; set; } = EdgeType.None;
        public EdgeType BottomEdgeType { get; set; } = EdgeType.None;
        public EdgeType LeftEdgeType { get; set; } = EdgeType.None;

        public CenterType CenterType = CenterType.None;

        public int RotationsCount { get; set; }

        public Card(string cardType, int cardNumber)
        {
            Dictionary<char, EdgeType> nameDict = new Dictionary<char, EdgeType>()
            {
                { 'R', EdgeType.Road },
                { 'C', EdgeType.Castle },
                { 'F', EdgeType.Cornfield },
                { 'W', EdgeType.Water }
            };

            CardId = $"{cardType}({cardNumber})";
            CardType = cardType;
            CardNumber = cardNumber;
            TopEdgeType = nameDict[cardType[0]];
            RightEdgeType = nameDict[cardType[1]];
            BottomEdgeType = nameDict[cardType[2]];
            LeftEdgeType = nameDict[cardType[3]];
        }

        public ObjectPart GetPart(string partName)
        {
            return Parts.Single(p => p.PartName == partName);
        }

        public void AddBorderToPart(Field field, FieldSide side, ObjectPart part, FieldBoard fieldBoard)
        {
            var rotatedSide = RotateSide(side, RotationsCount);
            var castleBorder = new Border(field, fieldBoard.GetNeighbour(field, rotatedSide), this);
            part.Borders.Add(castleBorder);
        }

        public List<string> GetConnectedCastleParts(CornfieldPart part)
        {
            var castleParts = new List<string>();
            if (FieldToCastleParts.Keys.Contains(part.PartId))
            {
                castleParts = FieldToCastleParts[part.PartId];
            }

            return castleParts;
        }

        public abstract void ConnectField(Field field, FieldBoard fieldBoard);

        // поворачивает карту на 90 по часовой стрелке градусов счетчик поворотов увеличивается на 1
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
        public static FieldSide RotateSide(FieldSide side, int rotationCount)
        {
            var result = ((byte)side + rotationCount) % 4;
            return (FieldSide)result;
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