using System;
using Carcassone.Core.Cards;

namespace Carcassone.Core.Fields
{
    /// <summary>
    /// Field on a game board, can contain card.
    /// </summary>
    public class Field
    {
        public Field(int x, int y)
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