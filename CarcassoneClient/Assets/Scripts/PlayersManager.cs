using Carcassone.Core.Players;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Хранит игроков
    /// </summary>
    public class PlayersManager
    {
        private const string _fileName = "LocalUsers.txt";

        public static void Save(List<Player> Users)
        {
            var json = JsonConvert.SerializeObject(Users);
            File.WriteAllText(GetFilePath(), json);
        }

        public static List<Player> Load()
        {
            try
            {
                var json = File.ReadAllText(GetFilePath());
                var users = JsonConvert.DeserializeObject<List<Player>>(json) ?? throw new Exception($"Can't load Local Users");
                return users;
            }
            catch
            {
                // создаем дефолтный список
                var dafaultUsersList = new List<Player>
                {
                    new Player() { Name = "AnonimusUser", PlayerType = PlayerType.Human },
                    new Player() { Name = "AI_Easy", PlayerType = PlayerType.AI_Easy },
                    new Player() { Name = "AI_Normal", PlayerType = PlayerType.AI_Normal },
                    new Player() { Name = "AI_Hard", PlayerType = PlayerType.AI_Hard }
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
