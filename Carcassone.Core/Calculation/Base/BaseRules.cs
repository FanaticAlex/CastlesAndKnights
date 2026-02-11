using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Monasteries;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Calculation.Base.Tiles;
using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carcassone.Core.Calculation.Base
{
    public class BaseRules: IGameRules
    {
        public List<IGameObjectsManager> Managers { get; } = new List<IGameObjectsManager>();

        public BaseRules(Grid grid)
        {
            var citiesManager = new CitiesManager();
            Managers.Add(citiesManager);
            Managers.Add(new RoadsManager());
            Managers.Add(new FarmsManager(citiesManager));
            Managers.Add(new MonasteriesManager(grid));
        }

        /// <summary>
        /// Base rules of connecting tiles
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="tile"></param>
        /// <param name="grid"></param>
        /// <param name="tileStack"></param>
        /// <returns></returns>
        public bool CanPutTileInCell(Cell cell, Tile tile, Grid grid)
        {
            if (cell.IsContainingTile()) return false; // if there is a tile already

            Tile? neighbourTopTile = grid.GetNeighbour(cell, Side.top)?.Tile;
            Tile? neighbourLeftTile = grid.GetNeighbour(cell, Side.left)?.Tile;
            Tile? neighbourBottomTile = grid.GetNeighbour(cell, Side.bottom)?.Tile;
            Tile? neighbourRightTile = grid.GetNeighbour(cell, Side.right)?.Tile;

            // карту можно положить в поле, если в соседних с полем областях либо нет карт
            // либо границы карты которую кладем и соседней карты совпадают
            if ((neighbourTopTile == null || neighbourTopTile.BottomEdgeType == tile.TopEdgeType) &&
                (neighbourLeftTile == null || neighbourLeftTile.RightEdgeType == tile.LeftEdgeType) &&
                (neighbourBottomTile == null || neighbourBottomTile.TopEdgeType == tile.BottomEdgeType) &&
                (neighbourRightTile == null || neighbourRightTile.LeftEdgeType == tile.RightEdgeType))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Base set of tiles
        /// </summary>
        /// <param name="stack"></param>
        public void AddTiles(TileStack stack)
        {
            stack.CreateTiles(typeof(CCCC), 1, 10);
            stack.CreateTiles(typeof(CCFC), 3, 10);
            stack.CreateTiles(typeof(CCFC_1), 1, 10);
            stack.CreateTiles(typeof(CCFF), 2, 10);
            stack.CreateTiles(typeof(CCRC), 1, 10);
            stack.CreateTiles(typeof(CCRC_1), 2, 10);
            stack.CreateTiles(typeof(CFFC), 3, 10);
            stack.CreateTiles(typeof(CFFC_1), 2, 10);
            stack.CreateTiles(typeof(CFFF), 5, 10);
            stack.CreateTiles(typeof(CFRR), 3, 10);
            stack.CreateTiles(typeof(CRFR), 3, 10);
            stack.CreateTiles(typeof(CRRC), 3, 10);
            stack.CreateTiles(typeof(CRRC_1), 2, 10);
            stack.CreateTiles(typeof(CRRF), 3, 10);
            stack.CreateTiles(typeof(CRRR), 3, 10);

            stack.CreateTiles(typeof(FCFC), 3, 10);
            stack.CreateTiles(typeof(FCFC_1), 1, 10);
            stack.CreateTiles(typeof(FCFC_2), 2, 10);
            stack.CreateTiles(typeof(FFFF), 4, 10);
            stack.CreateTiles(typeof(FFRF), 2, 10);
            stack.CreateTiles(typeof(FFRR), 8, 10);
            stack.CreateTiles(typeof(FRRR), 4, 10);

            stack.CreateTiles(typeof(RFRF), 8, 10);
            stack.CreateTiles(typeof(RRRR), 1, 10);
        }
    }
}
