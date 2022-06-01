using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carcassone.Core.Fields;

namespace Carcassone.Core.Cards
{
    /// <summary>
    /// Игровая карта. определяет местность на поле.
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Вручную соединенные замки и поля
        /// </summary>
        protected Dictionary<CornfieldPart, List<CastlePart>> _fieldToCastleParts =
            new Dictionary<CornfieldPart, List<CastlePart>>();

        /// <summary>
        /// Части обьектов на этой карте
        /// </summary>
        public List<ObjectPart> Parts { get; set; } = new List<ObjectPart>();

        /// <summary>
        /// Название карты, например СССС_1.
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        /// Поле в котором установлена карта.
        /// </summary>
        public Field Field { get; set; }

        public EdgeType TopEdgeType { get; set; } = EdgeType.None;
        public EdgeType RightEdgeType { get; set; } = EdgeType.None;
        public EdgeType BottomEdgeType { get; set; } = EdgeType.None;
        public EdgeType LeftEdgeType { get; set; } = EdgeType.None;

        public CenterType CenterType = CenterType.None;

        public int RotationsCount { get; set; } = 0;

        public Card(string cardName)
        {
            Dictionary<char, EdgeType> nameDict = new Dictionary<char, EdgeType>()
            {
                { 'R', EdgeType.Road },
                { 'C', EdgeType.Castle },
                { 'F', EdgeType.Cornfield },
                { 'W', EdgeType.Water }
            };

            CardName = cardName;
            TopEdgeType = nameDict[cardName[0]];
            RightEdgeType = nameDict[cardName[1]];
            BottomEdgeType = nameDict[cardName[2]];
            LeftEdgeType = nameDict[cardName[3]];
        }

        public List<CastlePart> GetConnectedCastleParts(CornfieldPart part)
        {
            var castleParts = new List<CastlePart>();
            if (_fieldToCastleParts.Keys.Contains(part))
            {
                castleParts = _fieldToCastleParts[part];
            }

            return castleParts;
        }

        public virtual void ConnectField(Field field)
        {
            Field = field;
        }

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
            RotationsCount = RotationsCount % 4;
        }

        /// <summary>
        /// Трансформация исходной стороны с учетом поворота карты
        /// </summary>
        /// <param name="side"></param>
        /// <param name="rotationCount"></param>
        /// <returns></returns>
        public static Side RotateSide(Side side, int rotationCount)
        {
            var result = ((byte)side + rotationCount) % 4;
            return (Side)result;
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