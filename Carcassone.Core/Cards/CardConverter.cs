using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Linq;

namespace Carcassone.Core.Cards
{
    internal class CardConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Card).IsAssignableFrom(objectType);
        }

        public override object? ReadJson(
            JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JToken card = JToken.Load(reader);
            if (!card.HasValues)
                return null;

            var cardType = card[nameof(Card.CardType)]?.ToString();
            if (cardType == null)
                throw new Exception($"Card type is not set {card}");

            Assembly asm = typeof(CardConverter).Assembly;
            Type type = asm.GetTypes().First(t => t.Name == cardType);
            return card.ToObject(type);
        }

        public override void WriteJson(
            JsonWriter writer, object? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
