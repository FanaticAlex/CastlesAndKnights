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
        private ChurchPart _churchPart;
        private CornfieldPart _cornfieldPart1;

        public FFFF(string cardName) : base(cardName)
        {
            _churchPart = new ChurchPart("Church_0", cardName);
            Parts.Add(_churchPart);

            _cornfieldPart1 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart1);
        }

        public override void ConnectField(Field field)
        {
            Field = field;

            _churchPart.ChurchField = field;

            AddBorderToPart(Side.top, _cornfieldPart1);
            AddBorderToPart(Side.right, _cornfieldPart1);
            AddBorderToPart(Side.bottom, _cornfieldPart1);
            AddBorderToPart(Side.left, _cornfieldPart1);
        }
    }
}