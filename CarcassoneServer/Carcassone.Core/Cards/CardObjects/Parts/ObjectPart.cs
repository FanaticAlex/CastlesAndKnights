using Carcassone.Core.Players;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
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
        public List<Border> Borders = new List<Border>();

        public Chip? Chip { get; set; }
        public Flag? Flag { get; set; }

        public string PartId { get; set; }
        public string PartName { get; set; }
        public string CardName { get; set; }
        public string PartType { get; set; }
        public bool IsOwned { get; set; }

        public ObjectPart(string partName, string cardName)
        {
            PartId = cardName + partName;
            CardName = cardName;
            PartName = partName;
            PartType = string.Empty;
        }
    }
}