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

        public static void Clear()
        {
            if (LoggerGameObject == null)
                LoggerGameObject = GameObject.Find("Log");

            LoggerMessages.Clear();
            LoggerGameObject.GetComponent<TMP_Text>().text = string.Empty;
        }

        public static void Info(string message)
        {
            if (LoggerGameObject == null)
                LoggerGameObject = GameObject.Find("Log");

            LoggerMessages.Add(message);
            LoggerGameObject.GetComponent<TMP_Text>().text = String.Join("\r\n", LoggerMessages.TakeLast(10));
        }
    }
}
