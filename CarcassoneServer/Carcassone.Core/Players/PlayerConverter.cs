using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using Carcassone.Core.Players.AI;

namespace Carcassone.Core.Players
{
    internal class PlayerConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(BasePlayer).IsAssignableFrom(objectType);
        }

        public override object? ReadJson(JsonReader reader,
            Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JToken player = JToken.Load(reader);
            if (!player.HasValues)
                return null;

            var playerName = player[nameof(Player.Name)]?.ToString();
            if (playerName == null)
                throw new Exception($"Player has no name {player}");

            if (playerName.Contains(PlayersPool.EasyBotName))
                return player.ToObject<PlayerAI>();
            else
                return player.ToObject<Player>();

            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer,
            object? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
