using Carcassone.Core.Board;
using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carcassone.Core.Calculation
{
    internal interface IGameExtension
    {
        bool CanPutCardInField(Cell cell, Tile tile, Grid grid, TileStack tileStack);
        public void AddTiles(TileStack stack);
    }
}
