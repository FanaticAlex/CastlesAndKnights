using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Calculation.Base.Monasteries;
using Carcassone.Core.Calculation.RiverExtension.Rivers;

namespace Carcassone.Core.Calculation
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
                case "City": return part.ToObject<CityPart>();
                case "Monastery": return part.ToObject<MonasteryPart>();
                case "Farm": return part.ToObject<FarmPart>();
                case "Road": return part.ToObject<RoadPart>();
                case "River": return part.ToObject<RiverPart>();
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
