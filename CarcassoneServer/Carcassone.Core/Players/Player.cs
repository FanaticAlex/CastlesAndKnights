using System.Linq;

namespace Carcassone.Core.Players
{
    public class Player : BasePlayer
    {
        public Player(string name, string color, int chipCount)
            : base(name, color, chipCount)
        {
        }

        public void SetPlayerMove1(GameRoom room, string cardId, string fieldId) // положить карту
        {
            var card = room.GetCard(cardId);
            var field = room.FieldBoard.GetField(fieldId);
            LastCardId = card.CardId;
            room.PutCardInField(card, field);
        }

        public void SetPlayerMove2(GameRoom room, string cardId, string partId) // положить фишку
        {
            var card = room.CardsPool.AllCards.First(_card => _card.CardId == cardId);
            var partObject = card.Parts.First(_part => _part.PartId == partId);
            room.PutChipInCard(partObject, Name);
        }

        public void SetPlayerMove3(GameRoom room) // завершить ход
        {
            room.EndTurn();
        }
    }
}