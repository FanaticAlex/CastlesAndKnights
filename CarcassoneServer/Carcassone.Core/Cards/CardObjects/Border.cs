using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{
    /// <summary>
    /// граница двух полей.
    /// </summary>
    public class Border
    {
        public Field FirstField { get; private set; }
        public Field SecondField { get; private set; }
        public CornfieldSide? cornfieldSide { get; set; }
        public Card Card { get; set; }

        public Border(Field first, Field second, Card card)
        {
            FirstField = first;
            SecondField = second;
            Card = card;
        }

        public static bool Equial(Border border1, Border border2)
        {
            if (border1.FirstField == null || border1.SecondField == null)
                return false;

            if (border2.FirstField == null || border2.SecondField == null)
                return false;

            if ((border1.FirstField == border2.FirstField) &&
                (border1.SecondField == border2.SecondField))
            {
                return true;
            }

            if ((border1.FirstField == border2.SecondField) &&
                (border1.SecondField == border2.FirstField))
            {
                return true;
            }

            return false;
        }
    }
}