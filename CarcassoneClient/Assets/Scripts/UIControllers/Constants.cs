using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public static class Constants
    {
        public static GameObject Chip => (GameObject)Resources.Load("Chip/Chip", typeof(GameObject));

        public static GameObject Flag => (GameObject)Resources.Load("Flags/Flag", typeof(GameObject));

        /// <summary>
        /// Рамочки игроков обозначающие последний ход
        /// </summary>
        public static Dictionary<string, GameObject> Marks = new Dictionary<string, GameObject>()
        {
            {"Blue", (GameObject)Resources.Load("Marks/mark_blue", typeof(GameObject))},
            {"Green", (GameObject)Resources.Load("Marks/mark_green", typeof(GameObject))},
            {"Red", (GameObject)Resources.Load("Marks/mark_red", typeof(GameObject))},
            //{"White", (GameObject)Resources.Load("Marks/mark_white", typeof(GameObject))},
            {"Yellow", (GameObject)Resources.Load("Marks/mark_yellow", typeof(GameObject))},
            {"Gray", (GameObject)Resources.Load("Marks/mark_gray", typeof(GameObject))}
        };

        public static Dictionary<string, Color> Colors = new Dictionary<string, Color>()
        {
            {"Blue", Color.blue},
            {"Green", Color.green},
            {"Red", Color.red},
            //{"White", Color.white},
            {"Yellow", Color.yellow},
            {"Gray", Color.gray}
        };
    }
}
