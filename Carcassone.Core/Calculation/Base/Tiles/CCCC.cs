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
            var CityPart = new CityPart(cityPartName, Id);
            Parts.Add(CityPart);
        }

        public override void ConnectCell(Cell cell, Grid grid)
        {
            AddBorderToPart(cell, CellSide.top, GetPart(cityPartName), grid);
            AddBorderToPart(cell, CellSide.right, GetPart(cityPartName), grid);
            AddBorderToPart(cell, CellSide.bottom, GetPart(cityPartName), grid);
            AddBorderToPart(cell, CellSide.left, GetPart(cityPartName), grid);
        }
    }
}