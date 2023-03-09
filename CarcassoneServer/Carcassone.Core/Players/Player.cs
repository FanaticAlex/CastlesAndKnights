using System.Linq;

namespace Carcassone.Core.Players
{
    public class Player : BasePlayer
    {
        private string? _cardName;
        private string? _fieldId;
        private string? _partId;
        public bool _turnEnded;

        public Player(string name, string color, int chipCount)
            : base(name, color, chipCount)
        {
        }

        public void SetPlayerMove1(string cardName, string fieldId) // положить карту
        {
            _cardName = cardName;
            _fieldId = fieldId;
        }

        public void SetPlayerMove2(string cardName, string partId) // положить фишку
        {
            _cardName = cardName;
            _partId = partId;
        }

        public void SetPlayerMove3() // завершить ход
        {
            _turnEnded = true;
        }

        public override void ProcessMove(GameRoom room)
        {
            while (true)
            {
                if (_cardName != null && _fieldId != null)
                {
                    var card = room.GetCard(_cardName);
                    var field = room.GetField(_fieldId);
                    LastCardId = card.CardName;
                    room.PutCardInField(card, field);
                    break;
                }
            }

            while (true)
            {
                if (_turnEnded)
                    break;

                if (_cardName != null && _partId != null)
                {
                    var card = room.GetAllCards().First(_card => _card.CardName == _cardName);
                    var partObject = card.Parts.First(_part => _part.PartId == _partId);
                    room.PutChipInCard(partObject, this);
                    break;
                }
            }

            while (true)
            {
                if (_turnEnded)
                {
                    _cardName = null;
                    _partId = null;
                    _turnEnded = false;
                    break;
                }
            }
        }
    }
}