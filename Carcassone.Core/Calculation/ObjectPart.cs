using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System.Collections.Generic;

namespace Carcassone.Core.Calculation
{
    /// <summary>
    /// Часть обьекта дорога, замок, поле или церковь.
    /// </summary>
    public abstract class ObjectPart
    {
        /// <summary>
        /// Список границ части игрового объекта,
        /// граница определяется полем на котором лежит карта с частью и соседним полем
        /// Границы есть только частей присоединенных карт
        /// </summary>
        public List<TileBorder> Borders = new List<TileBorder>();

        public Chip? Chip { get; set; }
        public Flag? Flag { get; set; }

        public string PartId { get; set; }
        public string PartName { get; set; }
        public string CardId { get; set; }
        public string PartType { get; set; }
        public bool IsPartOfOwnedObject { get; set; }

        public List<Side> Sides { get; set; } = new List<Side>();

        public ObjectPart(string partName, string cardId)
        {
            PartId = cardId + partName;
            CardId = cardId;
            PartName = partName;
            PartType = string.Empty;
        }
    }
}