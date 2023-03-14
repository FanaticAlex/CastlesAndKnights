using Carcassone.Core.Fields;

namespace Carcassone.Core.Cards
{
    /// <summary>
    /// Граница на карте
    /// </summary>
    public class Border
    {
        public Field FirstField { get; }

        public Field SecondField { get; }

        public CornfieldSide? CornfieldSide { get; set; }

        public Card Card { get; }

        public Border(Field? first, Field? second, Card card)
        {
            if (first == null || second == null)
                throw new System.Exception($"Field can not be null. Card: {card.CardName}, first: {first?.Id}, second: {second?.Id}");

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