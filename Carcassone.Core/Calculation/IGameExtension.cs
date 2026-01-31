using Carcassone.Core.Board;
using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carcassone.Core.Calculation
{
    internal interface IGameExtension
    {
        List<IGameObjectsManager> Managers { get; }
        bool CanPutTileInCell(Cell cell, Tile tile, Grid grid);
        public void AddTiles(TileStack stack);
    }
}
