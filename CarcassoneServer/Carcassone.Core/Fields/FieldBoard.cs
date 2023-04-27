using Carcassone.Core.Cards;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Fields
{
    /// <summary>
    /// Contains fields a methods of operating with fields.
    /// </summary>
    public class FieldBoard
    {
        private object _fieldsLock = new object();

        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public List<Field> Fields { get; set; }

        public FieldBoard()
        {
            Fields = new List<Field>();
            CreateField(0, 0);
        }

        public void PutCard(Card card, Field field)
        {
            lock (_fieldsLock)
            {
                // create 4 new fields around placed card
                CreateField(field.X, field.Y + 1);
                CreateField(field.X, field.Y - 1);
                CreateField(field.X + 1, field.Y);
                CreateField(field.X - 1, field.Y);

                field.CardName = card.CardId;
                card.ConnectField(field, this);
            }
        }

        public Field? GetNeighbour(Field? field, FieldSide side)
        {
            if (field == null)
                return null;

            return side switch
            {
                FieldSide.top => GetField(field.X, field.Y + 1),
                FieldSide.bottom => GetField(field.X, field.Y - 1),
                FieldSide.right => GetField(field.X + 1, field.Y),
                FieldSide.left => GetField(field.X - 1, field.Y),
                _ => throw new KeyNotFoundException(),
            };
        }

        public List<Field> GetAvailableFields()
        {
            return Fields.ToList().Where(f => string.IsNullOrEmpty(f.CardName)).ToList();
        }

        public Field? GetField(int x, int y) => GetField(Field.GetFieldID(x,y));

        public Field? GetField(string fieldId) => Fields.ToList().FirstOrDefault(field => field.Id == fieldId);

        private void CreateField(int x, int y)
        {
            var field = GetField(x, y);
            if (field == null)
                Fields.Add(new Field(x, y));
        }
    }
}
