using Carcassone.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Menu
{
    class PlayerTypeHelper
    {
        public static PlayerType ToPlayerType(string typeStr)
        {
            return typeStr.ToLower() switch
            {
                "human" => PlayerType.Human,
                "ai_easy" => PlayerType.AI_Easy,
                "ai_normal" => PlayerType.AI_Normal,
                "ai_hard" => PlayerType.AI_Hard,
                _ => throw new Exception($"type {typeStr} does not exist"),
            };
        }

        public static string ToString(PlayerType type)
        {
            return type switch
            {
                PlayerType.Human => "Human",
                PlayerType.AI_Easy => "AI_easy",
                PlayerType.AI_Normal => "AI_normal",
                PlayerType.AI_Hard => "AI_hard",
                _ => throw new Exception($"type {type} does not exist"),
            };
        }

        public static List<string> GetStrings()
        {
            return Enum.GetNames(typeof(PlayerType)).ToList();
        }
    }
}
