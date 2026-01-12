using Carcassone.Core.Tiles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Board
{
    /// <summary>
    /// Represents the game board where players put tiles
    /// </summary>
    public class Grid
    {
        private object _lockObj = new object();

        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public List<Cell> Cells { get; set; }

        public Grid()
        {
            Cells = new List<Cell>();
            CreateCell(0, 0); // create initial cell
        }

        public void PutCard(Tile card, Cell cell)
        {
            lock (_lockObj)
            {
                // create 4 new cells around placed card
                CreateCell(cell.X, cell.Y + 1);
                CreateCell(cell.X, cell.Y - 1);
                CreateCell(cell.X + 1, cell.Y);
                CreateCell(cell.X - 1, cell.Y);

                // connect card to cell
                cell.CardName = card.Id;
                card.ConnectField(cell, this);
            }
        }

        public Cell? GetNeighbour(Cell? field, CellSide side)
        {
            if (field == null)
                return null;

            return side switch
            {
                CellSide.top => GetFieldWithoutThrowing(field.X, field.Y + 1),
                CellSide.bottom => GetFieldWithoutThrowing(field.X, field.Y - 1),
                CellSide.right => GetFieldWithoutThrowing(field.X + 1, field.Y),
                CellSide.left => GetFieldWithoutThrowing(field.X - 1, field.Y),
                _ => throw new KeyNotFoundException(),
            };
        }

        public Cell? GetNeighbour(Cell? field, int deltaX, int deltaY)
        {
            if (field == null)
                return null;

            return GetFieldWithoutThrowing(field.X + deltaX, field.Y + deltaY);
        }

        public List<Cell> GetEmptyFields()
        {
            return Cells.ToList().Where(f => string.IsNullOrEmpty(f.CardName)).ToList();
        }

        public List<Cell> GetAvailableCells()
        {
            return Cells.ToList().Where(f => !f.NotAvailable).ToList();
        }

        public List<Cell> GetUnavailableFields()
        {
            return Cells.ToList().Where(f => f.NotAvailable).ToList();
        }

        public Cell GetField(int x, int y) => GetCell(Cell.GetFieldID(x,y));

        public Cell GetCell(string fieldId)
        {
            var field = Cells.ToList().FirstOrDefault(field => field.Id == fieldId);
            if (field == null)
                throw new Exception($"No field {fieldId} found");

            return field;
        }

        private void CreateCell(int x, int y)
        {
            var field = GetFieldWithoutThrowing(x, y);
            if (field == null)
                Cells.Add(new Cell(x, y));
        }

        private Cell GetFieldWithoutThrowing(int x, int y)
        {
            return Cells.ToList().FirstOrDefault(field => field.Id == Cell.GetFieldID(x, y));
        }
    }
}
