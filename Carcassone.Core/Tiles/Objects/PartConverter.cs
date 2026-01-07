using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;

namespace Carcassone.Core.Tiles.Objects
{
    internal class PartConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ObjectPart).IsAssignableFrom(objectType);
        }

        public override object? ReadJson(JsonReader reader,
            Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JToken part = JToken.Load(reader);
            if (!part.HasValues)
                return null;

            var partType = part[nameof(ObjectPart.PartType)]?.ToString();
            if (partType == null)
                throw new Exception($"Part type is not set {part}");

            switch (partType)
            {
                case "Castle": return part.ToObject<CastlePart>();
                case "Church": return part.ToObject<ChurchPart>();
                case "Cornfield": return part.ToObject<CornfieldPart>();
                case "Road": return part.ToObject<RoadPart>();
            }

            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
