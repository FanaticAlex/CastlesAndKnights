using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.Data.Common;
using System.Reflection;
using System.Linq;
using Carcassone.Core.Cards;
using Carcassone.Core.Players.AI;

namespace Carcassone.Core.Players
{
    internal class PlayerConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(BasePlayer).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader,
            Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken player = JToken.Load(reader);
            var playerName = player["Name"].ToString();
            
            if (playerName.Contains(PlayersPool.EasyBotName))
                return player.ToObject<PlayerAI>();
            else
                return player.ToObject<Player>();

            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
