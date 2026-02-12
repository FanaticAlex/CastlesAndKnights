using System;
using System.Drawing;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Board
{
    /// <summary>
    /// Cell on a board grid, can contain card.
    /// </summary>
    /*public class Cell
    {
        public Cell(int x, int y)
        {
            Id = GetCellID(x, y);
            Location = new Point(x, y);
            NotAvailable = false;
        }

        public string Id { get; }
        public Point Location { get; }
        public Tile Tile { get; set; }
        public bool NotAvailable { get; set; }

        public static string GetCellID(int x, int y) => $"{x}_{y}";

        public bool IsContainingTile() => (Tile != null);
    }*/
}