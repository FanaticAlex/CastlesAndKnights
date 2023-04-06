using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    internal class Logger
    {
        private static List<string> LoggerMessages { get; set; } = new List<string>();
        private static GameObject LoggerGameObject { get; set; }

        public static void Info(string message)
        {
            if (LoggerGameObject == null)
                LoggerGameObject = GameObject.Find("Log");

            LoggerMessages.Add(message);
            LoggerGameObject.GetComponent<TMP_Text>().text = String.Join("\r\n", LoggerMessages.TakeLast(10));
        }

        private static DateTime _time;
        public static void SaveTime(DateTime time)
        {
            _time = time;
        }

        public static void PrintSpendedTime(string message)
        {
            var timeDelta = System.DateTime.Now - _time;
            if (timeDelta > System.TimeSpan.FromSeconds(0.05))
                Logger.Info($"{message}: {timeDelta.TotalSeconds}c");
        }
    }
}
