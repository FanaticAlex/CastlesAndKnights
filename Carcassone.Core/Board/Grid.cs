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

        public void PutTile(Tile card, Cell cell)
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
            }
        }

        public Cell? GetNeighbour(Cell? cell, Side side)
        {
            if (cell == null)
                return null;

            return side switch
            {
                Side.top => GetCellWithoutThrowing(cell.X, cell.Y + 1),
                Side.bottom => GetCellWithoutThrowing(cell.X, cell.Y - 1),
                Side.right => GetCellWithoutThrowing(cell.X + 1, cell.Y),
                Side.left => GetCellWithoutThrowing(cell.X - 1, cell.Y),
                _ => throw new KeyNotFoundException(),
            };
        }

        public Cell? GetNeighbour(Cell? cell, int deltaX, int deltaY)
        {
            if (cell == null)
                return null;

            return GetCellWithoutThrowing(cell.X + deltaX, cell.Y + deltaY);
        }

        public List<Cell> GetEmptyCells()
        {
            return Cells.ToList().Where(c => string.IsNullOrEmpty(c.CardName)).ToList();
        }

        public List<Cell> GetAvailableCells()
        {
            return Cells.ToList().Where(c => !c.NotAvailable).ToList();
        }

        public List<Cell> GetUnavailableCells()
        {
            return Cells.ToList().Where(c => c.NotAvailable).ToList();
        }

        public Cell GetCell(int x, int y) => GetCell(Cell.GetCellID(x,y));

        public Cell GetCell(string cellId)
        {
            var cell = Cells.ToList().FirstOrDefault(c => c.Id == cellId);
            if (cell == null)
                throw new Exception($"No cell {cellId} found");

            return cell;
        }

        private void CreateCell(int x, int y)
        {
            var cell = GetCellWithoutThrowing(x, y);
            if (cell == null)
                Cells.Add(new Cell(x, y));
        }

        private Cell GetCellWithoutThrowing(int x, int y)
        {
            return Cells.ToList().FirstOrDefault(c => c.Id == Cell.GetCellID(x, y));
        }
    }
}
