using Carcassone.Core.Fields;

namespace Carcassone.Core.Cards
{
    /// <summary>
    ///       F
    ///   |       |
    /// F |   O   | F
    ///   |       |
    ///       F
    /// </summary>
    public class FFFF : Card
    {
        protected string churchPartName = "Church_0";
        protected string cornfieldPartName = "Cornfield_0";

        public FFFF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var churchPart = new ChurchPart(churchPartName, CardId);
            Parts.Add(churchPart);

            var cornfieldPart1 = new CornfieldPart(cornfieldPartName, CardId);
            Parts.Add(cornfieldPart1);
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            ((ChurchPart)GetPart(churchPartName)).ChurchFieldId = field.Id;

            AddBorderToPart(field, Side.top, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, Side.right, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, Side.bottom, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, Side.left, GetPart(cornfieldPartName), fieldBoard);
        }
    }
}