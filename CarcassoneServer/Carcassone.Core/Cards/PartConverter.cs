using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.Data.Common;
using System.Reflection;
using System.Linq;
using Carcassone.Core.Calculation.Objects;

namespace Carcassone.Core.Cards
{
    internal class PartConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ObjectPart).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader,
            Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken part = JToken.Load(reader);
            var partType = part["PartType"].ToString();
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
