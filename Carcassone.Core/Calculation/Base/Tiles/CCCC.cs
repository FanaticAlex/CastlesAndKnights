using System.Collections.Generic;
using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       C
    ///   |       |
    /// C |       | C
    ///   |       |
    ///       C
    /// </summary>
    public class CCCC : Tile
    {
        protected string cityPartName = "City_0";

        public CCCC(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var castlePart = new CityPart(cityPartName, Id);
            Parts.Add(castlePart);
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.top, GetPart(cityPartName), grid);
            AddBorderToPart(field, CellSide.right, GetPart(cityPartName), grid);
            AddBorderToPart(field, CellSide.bottom, GetPart(cityPartName), grid);
            AddBorderToPart(field, CellSide.left, GetPart(cityPartName), grid);
        }
    }
}