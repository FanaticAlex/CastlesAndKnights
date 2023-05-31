using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public static class Constants
    {
        /// <summary>
        /// Фишки игроков.
        /// </summary>
        public static GameObject Chip => (GameObject)Resources.Load("Chip/Chip", typeof(GameObject));

        /// <summary>
        /// Флажки законченных объектов.
        /// </summary>
        public static Dictionary<string, GameObject> Flags = new Dictionary<string, GameObject>()
        {
            {"Blue", (GameObject)Resources.Load("Flags/Flag_Blue", typeof(GameObject))},
            {"Green", (GameObject)Resources.Load("Flags/Flag_Green", typeof(GameObject))},
            {"Red", (GameObject)Resources.Load("Flags/Flag_Red", typeof(GameObject))},
            //{"White", (GameObject)Resources.Load("Flags/Flag_White", typeof(GameObject))},
            {"Yellow", (GameObject)Resources.Load("Flags/Flag_Yellow", typeof(GameObject))},
            {"Gray", (GameObject)Resources.Load("Flags/Flag_Gray", typeof(GameObject))}
        };

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
