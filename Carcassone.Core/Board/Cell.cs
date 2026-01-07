using System;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Board
{
    /// <summary>
    /// Cell on a board grid, can contain card.
    /// </summary>
    public class Cell
    {
        public Cell(int x, int y)
        {
            Id = GetFieldID(x, y);
            X = x;
            Y = y;
            NotAvailable = false;
        }

        public string Id { get; }
        public int X { get; }
        public int Y { get; }
        public string? CardName { get; set; }
        public bool NotAvailable { get; set; }

        public static string GetFieldID(int x, int y) => $"{x}_{y}";

        public bool IsContainsCard() => !string.IsNullOrEmpty(CardName);
    }
}