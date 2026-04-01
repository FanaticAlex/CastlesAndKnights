using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Carcassone.Core.Calculation
{
    /// <summary>
    /// Contain rules of the game.
    /// </summary>
    internal interface IGameRules
    {
        List<IGameObjectsManager> Managers { get; }
        bool CanPutTileInCell(Point location, int rotation, Tile tile, MoveState state);
        public void AddTiles(TileStack stack);
    }
}
