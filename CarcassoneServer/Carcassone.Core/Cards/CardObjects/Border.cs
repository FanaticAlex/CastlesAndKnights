using Carcassone.Core.Fields;

namespace Carcassone.Core.Cards
{
    /// <summary>
    /// Граница на карте
    /// </summary>
    public class Border
    {
        public string FirstFieldId { get; set; }

        public string SecondFieldId { get; set; }

        public CornfieldSide? CornfieldSide { get; set; }

        public string CardName { get; set; }

        public Border()
        {
        }

        public Border(Field? first, Field? second, Card card)
        {
            if (first == null || second == null)
                throw new System.Exception($"Field can not be null. Card: {card.CardId}, first: {first?.Id}, second: {second?.Id}");

            FirstFieldId = first.Id;
            SecondFieldId = second.Id;
            CardName = card.CardId;
        }

        public static bool Equial(Border border1, Border border2)
        {
            if (border1.FirstFieldId == null || border1.SecondFieldId == null)
                return false;

            if (border2.FirstFieldId == null || border2.SecondFieldId == null)
                return false;

            if ((border1.FirstFieldId == border2.FirstFieldId) &&
                (border1.SecondFieldId == border2.SecondFieldId))
            {
                return true;
            }

            if ((border1.FirstFieldId == border2.SecondFieldId) &&
                (border1.SecondFieldId == border2.FirstFieldId))
            {
                return true;
            }

            return false;
        }
    }
}