using System.Linq;

namespace Carcassone.Core.Players
{
    public class Player : BasePlayer
    {
        public Player(string name, string color, int chipCount)
            : base(name, color, chipCount)
        {
        }

        public void SetPlayerMove1(GameRoom room, string cardName, string fieldId) // положить карту
        {
            var card = room.GetCard(cardName);
            var field = room.GetField(fieldId);
            LastCardId = card.CardName;
            room.PutCardInField(card, field);
        }

        public void SetPlayerMove2(GameRoom room, string cardName, string partId) // положить фишку
        {
            var card = room.GetAllCards().First(_card => _card.CardName == cardName);
            var partObject = card.Parts.First(_part => _part.PartId == partId);
            room.PutChipInCard(partObject, this);
        }

        public void SetPlayerMove3(GameRoom room) // завершить ход
        {
            room.EndTurn();
        }
    }
}