using Carcassone.Core.Board;
using Carcassone.Core.Calculation.RiverExtension.Rivers;
using Carcassone.Core.Calculation.RiverExtension.Tiles;
using Carcassone.Core.Tiles;
using System.Collections.Generic;

namespace Carcassone.Core.Calculation.RiverExtension
{
    /// <summary>
    /// This extension adds river tiles to the game.
    /// 
    /// 1. 12 river tiles
    /// 2. there is only one river object in a game
    /// 3. game starts with composing river with river tiles and then as usual
    /// 4. river always starts with river source tile
    /// 5. river always ends with river mouth tile
    /// 6. all river cards should be used
    /// 7. river only flows down or left and never up or right
    /// </summary>
    public class RiverExtension: IGameExtension
    {
        public List<IGameObjectsManager> Managers { get; } = new List<IGameObjectsManager>();

        public RiverExtension()
        {
            Managers.Add(new RiversManager());
        }

        public bool CanPutTileInCell(Cell cell, Tile tile, Grid grid)
        {
            // направление реки всегда должно быть вниз и влево
            var isRiverCard = tile.Id.Contains("W");
            if (isRiverCard)
            {
                Tile? neighbourTopCard = grid.GetNeighbour(cell, Side.top)?.Tile;
                Tile? neighbourRightCard = grid.GetNeighbour(cell, Side.right)?.Tile;

                bool isTopFree = (neighbourTopCard == null);
                bool isRightFree = (neighbourRightCard == null);

                // проверям направление реки
                bool isWaterDirectionTop = (isTopFree && tile.TopEdgeType == 'W');
                bool isWaterDirectionRight = (isRightFree && tile.RightEdgeType == 'W');

                // водную карту можно положить в поле, если в соседних с полем областях либо нет карт
                // либо водные границы соседних карт совпадают
                if (isWaterDirectionTop || isWaterDirectionRight)
                {
                    return false;
                }
            }

            return true;
        }

        public void AddTiles(TileStack stack)
        {
            // river cards priority should be so that they always apear at the start of the tile stack

            stack.CreateTiles(typeof(FFWF), 1, 0); // river source tile

            stack.CreateTiles(typeof(CWCW), 1, 1);
            stack.CreateTiles(typeof(FWRW), 1, 1);
            stack.CreateTiles(typeof(FWWF), 1, 1);
            stack.CreateTiles(typeof(FWWF_1), 1, 1);
            stack.CreateTiles(typeof(RRWW), 1, 1);
            stack.CreateTiles(typeof(RWRW), 1, 1);
            stack.CreateTiles(typeof(WCCW), 1, 1);
            stack.CreateTiles(typeof(WCWR), 1, 1);
            stack.CreateTiles(typeof(WFWF), 1, 1);
            stack.CreateTiles(typeof(WFWF_1), 1, 1);

            stack.CreateTiles(typeof(WFFF), 1, 2); // river mouth tile
        }
    }
}
