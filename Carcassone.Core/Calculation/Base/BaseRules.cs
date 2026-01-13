using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Tiles;
using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carcassone.Core.Calculation.Base
{
    public class BaseRules: IGameExtension
    {
        public bool CanPutCardInField(Cell cell, Tile tile, Grid grid, TileStack tileStack)
        {
            if (cell.IsContainsCard()) return false; // if there is a tile already

            var neighbourTopCardName = grid.GetNeighbour(cell, CellSide.top)?.CardName;
            Tile? neighbourTopCard = neighbourTopCardName != null ? tileStack.GetCard(neighbourTopCardName) : null;
            var neighbourLeftCardName = grid.GetNeighbour(cell, CellSide.left)?.CardName;
            Tile? neighbourLeftCard = neighbourLeftCardName != null ? tileStack.GetCard(neighbourLeftCardName) : null;
            var neighbourBottomCardName = grid.GetNeighbour(cell, CellSide.bottom)?.CardName;
            Tile? neighbourBottomCard = neighbourBottomCardName != null ? tileStack.GetCard(neighbourBottomCardName) : null;
            var neighbourRightCardName = grid.GetNeighbour(cell, CellSide.right)?.CardName;
            Tile? neighbourRightCard = neighbourRightCardName != null ? tileStack.GetCard(neighbourRightCardName) : null;

            // карту можно положить в поле, если в соседних с полем областях либо нет карт
            // либо границы карты которую кладем и соседней карты совпадают
            if ((neighbourTopCard == null || neighbourTopCard.BottomEdgeType == tile.TopEdgeType) &&
                (neighbourLeftCard == null || neighbourLeftCard.RightEdgeType == tile.LeftEdgeType) &&
                (neighbourBottomCard == null || neighbourBottomCard.TopEdgeType == tile.BottomEdgeType) &&
                (neighbourRightCard == null || neighbourRightCard.LeftEdgeType == tile.RightEdgeType))
            {
                return true;
            }

            return false;
        }

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
