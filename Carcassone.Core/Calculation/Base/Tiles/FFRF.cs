using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Monasteries;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.Base.Tiles
{
    /// <summary>
    ///       F
    ///   |       |
    /// F |   O   | F
    ///   |   |   |
    ///       R
    /// </summary>
    public class FFRF : Tile
    {
        protected string roadPartName = "Road_0";
        protected string churchPartName = "Church_0";
        protected string cornfieldPartName = "Cornfield_0";

        public FFRF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var roadPart3 = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart3);

            var churchPart = new MonasteryPart(churchPartName, Id);
            Parts.Add(churchPart);

            var cornfieldPart1 = new FieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart1);
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.bottom, GetPart(roadPartName), grid);

            ((MonasteryPart)GetPart(churchPartName)).CellId = field.Id;

            AddBorderToPart(field, CellSide.top, GetPart(cornfieldPartName), grid);
            AddBorderToPart(field, CellSide.right, GetPart(cornfieldPartName), grid);
            AddBorderToPart(field, CellSide.left, GetPart(cornfieldPartName), grid);
            AddCornfieldSplittedBorder(field, CellSide.bottom, FieldSide.side_3, GetPart(cornfieldPartName), grid);
            AddCornfieldSplittedBorder(field, CellSide.bottom, FieldSide.side_4, GetPart(cornfieldPartName), grid);
        }
    }
}