using Carcassone.Core.Cards;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Fields
{
    /// <summary>
    /// Contains fields a methods of operating with fields.
    /// </summary>
    public class FieldBoard
    {
        private List<Field> Fields { get; } = new List<Field>();

        public FieldBoard()
        {
            CreateField(0, 0);
        }

        public List<Field> GetAvailableFields()
        {
            return Fields.Where(f => f.GetCardInField() == null).ToList();
        }

        public List<Field> GetAllFields()
        {
            return Fields;
        }

        public void CreateField(int x, int y)
        {
            var field = GetField(x, y);
            if (field == null)
                Fields.Add(new Field(this, x, y));
        }

        public void PutCard(Card card, Field field)
        {
            // создаем новые поля вокруг положенной карты
            CreateField(field.X, field.Y + 1);
            CreateField(field.X, field.Y - 1);
            CreateField(field.X + 1, field.Y);
            CreateField(field.X - 1, field.Y);

            field.SetCardInField(card);
            card.ConnectField(field);
        }

        public Field GetCenter() => GetField(0, 0);

        public Field? GetField(int x, int y) => GetField($"{x}_{y}");

        public Field? GetField(string fieldId) => Fields.FirstOrDefault(field => field.Id == fieldId);
    }
}
