using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.Base.Cities
{
    public class CityPart : ObjectPart
    {
        public CityPart(string partName, Tile tile, bool isThereShield = false)
            : base(partName, tile)
        {
            PartType = "City";
            IsThereShield = isThereShield;
        }

        /// <summary>
        /// Есть ли на карте города щит.
        /// </summary>
        public bool IsThereShield { get; set; }
    }
}