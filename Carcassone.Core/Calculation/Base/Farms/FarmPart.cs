using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Tiles;
using System.Collections.Generic;

namespace Carcassone.Core.Calculation.Base.Farms
{
    // farm part is also called field
    public class FarmPart : ObjectPart
    {
        /// <summary>
        /// Вручную соединенные замки и поля,
        /// это нужно для подсчета какие замки присоденены к полям при подсчете очков за поля
        /// </summary>
        public List<CityPart> ConnectedCityParts { get; set; } = new List<CityPart>();

        public FarmPart(string partName, Tile tile) : base(partName, tile)
        {
            PartType = "Farm";
        }
    }
}