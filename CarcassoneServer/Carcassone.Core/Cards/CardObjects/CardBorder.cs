using Carcassone.Core.Fields;
using Newtonsoft.Json;

namespace Carcassone.Core.Cards
{
    public class CardBorder
    {
        public string FirstFieldId { get; set; }

        public string SecondFieldId { get; set; }

        public CornfieldSide? CornfieldSide { get; set; }

        public string CardName { get; set; }

        [JsonConstructor]
        public CardBorder() { }

        public CardBorder(Field? first, Field? second, Card card)
        {
            if (first == null || second == null)
                throw new System.Exception($"Field can not be null. Card: {card.Id}, first: {first?.Id}, second: {second?.Id}");

            FirstFieldId = first.Id;
            SecondFieldId = second.Id;
            CardName = card.Id;
        }

        public static bool Equial(CardBorder border1, CardBorder border2)
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