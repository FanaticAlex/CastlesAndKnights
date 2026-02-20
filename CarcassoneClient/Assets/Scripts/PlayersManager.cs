using Carcassone.Core.Players;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class LocalPlayerInfo
    {
        public string Name { get; set; }
        public PlayerType Type { get; set; }
    }

    /// <summary>
    /// Хранит игроков
    /// </summary>
    public class PlayersManager
    {
        private const string _fileName = "LocalUsers.txt";

        public static void Save(List<LocalPlayerInfo> Users)
        {
            //var json = JsonConvert.SerializeObject(Users);
            //File.WriteAllText(GetFilePath(), json);
        }

        public static List<LocalPlayerInfo> Load()
        {
            //try
            //{
            //    var json = File.ReadAllText(GetFilePath());
            //    var users = JsonConvert.DeserializeObject<List<Player>>(json) ?? throw new Exception($"Can't load Local Users");
            //    return users;
            //}
            //catch
            {
                // создаем дефолтный список
                var dafaultUsersList = new List<LocalPlayerInfo>
                {
                    new LocalPlayerInfo() { Name = "AnonimusUser", Type = PlayerType.Human },
                    new LocalPlayerInfo() { Name = "AI_Easy", Type = PlayerType.AI_Easy },
                    new LocalPlayerInfo() { Name = "AI_Normal", Type = PlayerType.AI_Normal },
                    new LocalPlayerInfo() { Name = "AI_Hard", Type = PlayerType.AI_Hard }
                };
                Save(dafaultUsersList);
                return dafaultUsersList;
            }
        }

        private static string GetFilePath()
        {
            return Application.persistentDataPath + _fileName;
        }
    }
}
