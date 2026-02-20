using UnityEngine;
using System.Collections.Generic;
using Carcassone.Core.Players;

namespace Assets.Scripts
{
    public static class Constants
    {
        public static GameObject Chip => (GameObject)Resources.Load("Chip/Chip", typeof(GameObject));

        public static GameObject Flag => (GameObject)Resources.Load("Flags/Flag", typeof(GameObject));

        /// <summary>
        /// Рамочки игроков обозначающие последний ход
        /// </summary>
        public static Dictionary<PlayerColor, GameObject> Marks = new Dictionary<PlayerColor, GameObject>()
        {
            {PlayerColor.Blue, (GameObject)Resources.Load("Marks/mark_blue", typeof(GameObject))},
            {PlayerColor.Green, (GameObject)Resources.Load("Marks/mark_green", typeof(GameObject))},
            {PlayerColor.Red, (GameObject)Resources.Load("Marks/mark_red", typeof(GameObject))},
            //{PlayerColor.White, (GameObject)Resources.Load("Marks/mark_white", typeof(GameObject))},
            {PlayerColor.Yellow, (GameObject)Resources.Load("Marks/mark_yellow", typeof(GameObject))},
            {PlayerColor.Gray, (GameObject)Resources.Load("Marks/mark_gray", typeof(GameObject))}
        };

        public static Dictionary<PlayerColor, Color> Colors = new Dictionary<PlayerColor, Color>()
        {
            {PlayerColor.Blue, Color.blue},
            {PlayerColor.Green, Color.green},
            {PlayerColor.Red, Color.red},
            //{PlayerColor.White, Color.white},
            {PlayerColor.Yellow, Color.yellow},
            {PlayerColor.Gray, Color.gray}
        };
    }
}
