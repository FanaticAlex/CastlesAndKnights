using System;
using System.Collections.Generic;
using Carcassone.Core.Tiles;
using Carcassone.Core.Board;
using Carcassone.Core.Players;
using System.Linq;
using System.Drawing;

namespace Carcassone.Core.Calculation.Base.Monasteries
{
    public class Monastery : BaseGameObject, ICompletableGameObject
    {
        private readonly Grid _grid;
        private const int MaxConnectedTiles = 9;
        private Tile _monasteryTile;

        public Monastery(MonasteryPart basePart, Tile tile, Grid grid)
        {
            Parts.Add(basePart);
            _monasteryTile = tile;
            _grid = grid;
        }

        public override int GetScore()
        {
            // one neighbout tile = one score point
            return IsComplete() ? MaxConnectedTiles * 2 : GetNeighboutTilesCount();
        }

        public bool IsComplete()
        {
            return (GetNeighboutTilesCount() == MaxConnectedTiles);
        }

        /// <summary>
        /// завершить церковь если в ней 9 карт и вернуть фишку
        /// </summary>
        public void TryCompleteAndReturnChips()
        {
            if (IsComplete())
                Parts.First().Chip?.Owner?.ReturnChipAndSetFlag(Parts.First());
        }

        private int GetNeighboutTilesCount()
        {
            var tilesCount = 1; // bace monastery tile

            // are there cards in neighbour cells
            if (_grid.GetTile(_monasteryTile.Location + new Size(1, 0)) != null) tilesCount++;
            if (_grid.GetTile(_monasteryTile.Location + new Size(-1, 0)) != null) tilesCount++;
            if (_grid.GetTile(_monasteryTile.Location + new Size(0, 1)) != null) tilesCount++;
            if (_grid.GetTile(_monasteryTile.Location + new Size(0, -1)) != null) tilesCount++;
            if (_grid.GetTile(_monasteryTile.Location + new Size(1, 1)) != null) tilesCount++;
            if (_grid.GetTile(_monasteryTile.Location + new Size(-1, -1)) != null) tilesCount++;
            if (_grid.GetTile(_monasteryTile.Location + new Size(-1, 1)) != null) tilesCount++;
            if (_grid.GetTile(_monasteryTile.Location + new Size(1, -1)) != null) tilesCount++;

            return tilesCount;
        }
    }
}
