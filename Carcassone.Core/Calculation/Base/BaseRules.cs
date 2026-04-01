using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Monasteries;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Calculation.Base.Tiles;
using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Carcassone.Core.Calculation.Base
{
    public class BaseRules: IGameRules
    {
        public List<IGameObjectsManager> Managers { get; } = new List<IGameObjectsManager>();

        public BaseRules()
        {
            Managers.Add(new CitiesManager());
            Managers.Add(new RoadsManager());
            Managers.Add(new FarmsManager());
            Managers.Add(new MonasteriesManager());
        }

        /// <summary>
        /// Base rules of connecting tiles
        /// </summary>
        public bool CanPutTileInCell(Point location, int rotation, Tile tile, MoveState state)
        {
            var neighbourTopTile = state.GetNeighbour(location, Side.top);
            var neighbourLeftTile = state.GetNeighbour(location, Side.left);
            var neighbourBottomTile = state.GetNeighbour(location, Side.bottom);
            var neighbourRightTile = state.GetNeighbour(location, Side.right);

            // карту можно положить в поле, если в соседних с полем областях либо нет карт
            // либо границы карты которую кладем и соседней карты совпадают
            if ((neighbourTopTile == null || Tile.GetEdge(neighbourTopTile.TileId, Side.bottom, neighbourTopTile.TileRotation) == Tile.GetEdge(tile.TileType, Side.top, rotation)) &&
                (neighbourLeftTile == null || Tile.GetEdge(neighbourLeftTile.TileId, Side.right, neighbourLeftTile.TileRotation) == Tile.GetEdge(tile.TileType, Side.left, rotation)) &&
                (neighbourBottomTile == null || Tile.GetEdge(neighbourBottomTile.TileId, Side.top, neighbourBottomTile.TileRotation) == Tile.GetEdge(tile.TileType, Side.bottom, rotation)) &&
                (neighbourRightTile == null || Tile.GetEdge(neighbourRightTile.TileId, Side.left, neighbourRightTile.TileRotation) == Tile.GetEdge(tile.TileType, Side.right, rotation)))
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
            stack.CreateTiles(typeof(CCFC1), 1, 10);
            stack.CreateTiles(typeof(CCFF), 2, 10);
            stack.CreateTiles(typeof(CCRC), 1, 10);
            stack.CreateTiles(typeof(CCRC1), 2, 10);
            stack.CreateTiles(typeof(CFFC), 3, 10);
            stack.CreateTiles(typeof(CFFC1), 2, 10);
            stack.CreateTiles(typeof(CFFF), 5, 10);
            stack.CreateTiles(typeof(CFRR), 3, 10);
            stack.CreateTiles(typeof(CRFR), 3, 10);
            stack.CreateTiles(typeof(CRRC), 3, 10);
            stack.CreateTiles(typeof(CRRC1), 2, 10);
            stack.CreateTiles(typeof(CRRF), 3, 10);
            stack.CreateTiles(typeof(CRRR), 3, 10);

            stack.CreateTiles(typeof(FCFC), 3, 10);
            stack.CreateTiles(typeof(FCFC1), 1, 10);
            stack.CreateTiles(typeof(FCFC2), 2, 10);
            stack.CreateTiles(typeof(FFFF), 4, 10);
            stack.CreateTiles(typeof(FFRF), 2, 10);
            stack.CreateTiles(typeof(FFRR), 8, 10);
            stack.CreateTiles(typeof(FRRR), 4, 10);

            stack.CreateTiles(typeof(RFRF), 8, 10);
            stack.CreateTiles(typeof(RRRR), 1, 10);
        }
    }
}
