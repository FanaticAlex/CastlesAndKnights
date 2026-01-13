using Carcassone.Core.Board;
using Carcassone.Core.Calculation.RiverExtension.Tiles;
using Carcassone.Core.Tiles;
using System.Collections.Generic;

namespace Carcassone.Core.Calculation.RiverExtension
{
    /// <summary>
    /// This extension is adding river cards to the game.
    /// </summary>
    public class RiverExtension: IGameExtension
    {
        public bool CanPutCardInField(Cell cell, Tile tile, Grid grid, TileStack tileStack)
        {
            // направление реки всегда должно быть вниз и влево
            var isRiverCard = tile.Id.Contains("W");
            if (isRiverCard)
            {
                var neighbourTopCardName = grid.GetNeighbour(cell, CellSide.top)?.CardName;
                Tile? neighbourTopCard = neighbourTopCardName != null ? tileStack.GetCard(neighbourTopCardName) : null;

                var neighbourRightCardName = grid.GetNeighbour(cell, CellSide.right)?.CardName;
                Tile? neighbourRightCard = neighbourRightCardName != null ? tileStack.GetCard(neighbourRightCardName) : null;

                bool isTopFree = neighbourTopCard == null;
                bool isRightFree = neighbourRightCard == null;

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
            stack.CreateTiles(typeof(FFWF), 1, 0); // river start

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

            stack.CreateTiles(typeof(WFFF), 1, 2); // river end
        }
    }
}
