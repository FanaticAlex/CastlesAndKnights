using System;
using System.Collections.Generic;
using Carcassone.Core.Tiles;
using Carcassone.Core.Board;
using Carcassone.Core.Players;
using Newtonsoft.Json;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Monasteries
{
    public class Monastery : IClosingObject, IOwnedObject
    {
        private readonly Grid _grid;
        private const int MaxConnectedTiles = 9;
        private MonasteryPart _baseChurchPart;

        public bool IsFinished => GetNeighboutTilesCount() == MaxConnectedTiles;

        public Monastery(MonasteryPart basePart, Cell cell, Grid grid)
        {
            _baseChurchPart = basePart;
            _grid = grid;
        }

        public string? GetOwnerName(Stack cardPool)
        {
            return _baseChurchPart.Chip?.OwnerName ?? _baseChurchPart.Flag?.OwnerName;
        }

        public bool IsPlayerOwner(GamePlayer player, Stack cardPool)
        {
            return GetOwnerName(cardPool) == player.Name;
        }

        /// <summary>
        /// закрыть церковь если в ней 9 карт и вернуть фишку
        /// </summary>
        public void TryToClose(GamePlayersPool playersPool, Stack cardPool)
        {
            if (IsFinished)
            {
                if (_baseChurchPart.Chip?.OwnerName != null)
                {
                    var player = playersPool.GetPlayer(_baseChurchPart.Chip.OwnerName);
                    player.ReturnChipAndSetFlag(_baseChurchPart);
                }
            }
        }

        /// <summary>
        /// 1 карта = 1 очко. если завершено 1 карта = 2 очка.
        /// </summary>
        /// <returns></returns>
        public int GetPoints()
        {
            return IsFinished ? MaxConnectedTiles * 2 : GetNeighboutTilesCount();
        }

        private int GetNeighboutTilesCount()
        {
            var centralCell = _grid.GetCell(_baseChurchPart.CellId);
            var tilesCount = 1; // bace monastery tile

            // are there cards in neighbour cells
            if (!string.IsNullOrEmpty(_grid.GetNeighbour(centralCell, 1, 0)?.CardName)) tilesCount++;
            if (!string.IsNullOrEmpty(_grid.GetNeighbour(centralCell, -1, 0)?.CardName)) tilesCount++;
            if (!string.IsNullOrEmpty(_grid.GetNeighbour(centralCell, 0, 1)?.CardName)) tilesCount++;
            if (!string.IsNullOrEmpty(_grid.GetNeighbour(centralCell, 0, -1)?.CardName)) tilesCount++;
            if (!string.IsNullOrEmpty(_grid.GetNeighbour(centralCell, 1, 1)?.CardName)) tilesCount++;
            if (!string.IsNullOrEmpty(_grid.GetNeighbour(centralCell, -1, -1)?.CardName)) tilesCount++;
            if (!string.IsNullOrEmpty(_grid.GetNeighbour(centralCell, -1, 1)?.CardName)) tilesCount++;
            if (!string.IsNullOrEmpty(_grid.GetNeighbour(centralCell, 1, -1)?.CardName)) tilesCount++;

            return tilesCount;
        }
    }
}
