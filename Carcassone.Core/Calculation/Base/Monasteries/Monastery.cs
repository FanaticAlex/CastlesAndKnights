using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Monasteries
{
    public class Monastery : BaseGameObject, ICompletableGameObject
    {
        private const int MaxConnectedTiles = 9;
        private Tile _monasteryTile;

        public Monastery(MonasteryPart basePart, Tile tile)
        {
            Parts.Add(basePart);
            _monasteryTile = tile;
        }

        public override int GetScore(MoveState state)
        {
            // one neighbout tile = one score point
            return IsComplete(state) ? MaxConnectedTiles * 2 : GetNeighboutTilesCount(state);
        }

        public bool IsComplete(MoveState state)
        {
            return (GetNeighboutTilesCount(state) == MaxConnectedTiles);
        }

        private int GetNeighboutTilesCount(MoveState state)
        {
            var tilesCount = 1; // base monastery tile

            // are there cards in neighbour cells
            var location = state.GetTileLocation(_monasteryTile.Id);
            if (state.GetMove(location + new Size(1, 0)) != null) tilesCount++;
            if (state.GetMove(location + new Size(-1, 0)) != null) tilesCount++;
            if (state.GetMove(location + new Size(0, 1)) != null) tilesCount++;
            if (state.GetMove(location + new Size(0, -1)) != null) tilesCount++;
            if (state.GetMove(location + new Size(1, 1)) != null) tilesCount++;
            if (state.GetMove(location + new Size(-1, -1)) != null) tilesCount++;
            if (state.GetMove(location + new Size(-1, 1)) != null) tilesCount++;
            if (state.GetMove(location + new Size(1, -1)) != null) tilesCount++;

            return tilesCount;
        }

        public override BaseGameObject Copy()
        {
            var copy = new Monastery((MonasteryPart)Parts.First(), _monasteryTile);
            copy.Parts.AddRange(Parts);
            return copy;
        }
    }
}
