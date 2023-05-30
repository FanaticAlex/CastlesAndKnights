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

        public Card(string cardType, int cardNumber)
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

        public ObjectPart GetPart(string partName) => Parts.Single(p => p.PartName == partName);

        public void AddBorderToPart(Field field, FieldSide side, ObjectPart part, FieldBoard fieldBoard)
        {
            var rotatedSide = RotateSide(side, RotationsCount);
            var castleBorder = new CardBorder(field, fieldBoard.GetNeighbour(field, rotatedSide), this);
            part.Borders.Add(castleBorder);
        }

        public void AddCornfieldSplittedBorder(
            Field field, FieldSide cornfieldSide, CornfieldSide cornfieldSidePart, ObjectPart part, FieldBoard fieldBoard)
        {
            cornfieldSide = RotateSide(cornfieldSide, RotationsCount);
            cornfieldSidePart = RotateSidePart(cornfieldSidePart, RotationsCount);
            var cornfield1Border0 = new CardBorder(field, fieldBoard.GetNeighbour(field, cornfieldSide), this);
            cornfield1Border0.CornfieldSide = cornfieldSidePart;
            part.Borders.Add(cornfield1Border0);
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