using System;
using Carcassone.Core.Cards;

namespace Carcassone.Core.Fields
{
    /// <summary>
    /// Field on a game board, can contain card.
    /// </summary>
    public class Field
    {
        public Field(int x, int y)
        {
            Id = GetFieldID(x, y);
            X = x;
            Y = y;
        }

        public string Id { get; }
        public int X { get; }
        public int Y { get; }
        public string? CardName { get; set; }

        public static string GetFieldID(int x, int y) => $"{x}_{y}";

        public bool RotateCardTilFit(Card card, FieldBoard fieldBoard, CardPool cardPool)
        {
            for (int i = 0; i < 4; i++)
            {
                if (CanPutCardInThisField(card, fieldBoard, cardPool))
                {
                    return true;
                }
                else
                {
                    card.RotateCard();
                }
            }

            // доворачиваем до исходного положения если не подходит
            card.RotateCard();
            return false;
        }

        internal bool CanPutCardInThisFieldWithRotation(Card? card, FieldBoard fieldBoard, CardPool cardPool)
        {
            if (!string.IsNullOrEmpty(CardName))
                return false;

            if (card == null)
                return false;

            var type = card.GetType();
            var copy = (Card)Activator.CreateInstance(type, card.CardType, card.CardNumber);
            copy.TopEdgeType = card.TopEdgeType;
            copy.LeftEdgeType = card.LeftEdgeType;
            copy.BottomEdgeType = card.BottomEdgeType;
            copy.RightEdgeType = card.RightEdgeType;

            return RotateCardTilFit(copy, fieldBoard, cardPool);
        }

        internal bool CanPutCardInThisField(Card card, FieldBoard fieldBoard, CardPool cardPool)
        {
            // if there is another card in field then false
            if (!string.IsNullOrEmpty(CardName))
                return false;

            var neighbourTopCardName = fieldBoard.GetNeighbour(this, FieldSide.top)?.CardName;
            Card? neighbourTopCard = neighbourTopCardName != null ? cardPool.GetCard(neighbourTopCardName) : null;
            var neighbourLeftCardName = fieldBoard.GetNeighbour(this, FieldSide.left)?.CardName;
            Card? neighbourLeftCard = neighbourLeftCardName != null ? cardPool.GetCard(neighbourLeftCardName) : null;
            var neighbourBottomCardName = fieldBoard.GetNeighbour(this, FieldSide.bottom)?.CardName;
            Card? neighbourBottomCard = neighbourBottomCardName != null ? cardPool.GetCard(neighbourBottomCardName) : null;
            var neighbourRightCardName = fieldBoard.GetNeighbour(this, FieldSide.right)?.CardName;
            Card? neighbourRightCard = neighbourRightCardName != null ? cardPool.GetCard(neighbourRightCardName) : null;

            // если есть граничные карты то границы должны совпадать иначе карту присоединить нельзя
            var isRiverCard = card.Id.Contains("W");
            if (isRiverCard)
            {
                bool isTopFree = neighbourTopCard == null;
                bool isLeftFree = neighbourLeftCard == null;
                bool isBottomFree = neighbourBottomCard == null;
                bool isRightFree = neighbourRightCard == null;

                // направелние реки должно быть строго сверху вниз, поворачивать реку наверх нельзя
                bool isWaterDirectionCorrect = !(isTopFree && card.TopEdgeType == CardEdgeType.Water);

                bool connectWithTopWaterCard = (neighbourTopCard?.BottomEdgeType == card.TopEdgeType && card.TopEdgeType == CardEdgeType.Water);
                bool connectWithLeftWaterCard = (neighbourLeftCard?.RightEdgeType == card.LeftEdgeType && card.LeftEdgeType == CardEdgeType.Water);
                bool connectWithBottomWaterCard = (neighbourBottomCard?.TopEdgeType == card.BottomEdgeType && card.BottomEdgeType == CardEdgeType.Water);
                bool connectWithRightWaterCard = (neighbourRightCard?.LeftEdgeType == card.RightEdgeType && card.RightEdgeType == CardEdgeType.Water);

                // водную карту можно положить в поле, если в соседних с полем областях либо нет карт
                // либо водные границы соседних карт совпадают
                if ((isTopFree || connectWithTopWaterCard) &&
                    (isLeftFree || connectWithLeftWaterCard) &&
                    (isBottomFree || connectWithBottomWaterCard) &&
                    (isRightFree || connectWithRightWaterCard) &&
                    isWaterDirectionCorrect)
                {
                    return true;
                }
            }
            else
            {                
                // карту можно положить в поле, если в соседних с полем областях либо нет карт
                // либо границы карты которую кладем и соседней карты совпадают
                if ((neighbourTopCard == null || neighbourTopCard.BottomEdgeType == card.TopEdgeType) &&
                    (neighbourLeftCard == null || neighbourLeftCard.RightEdgeType == card.LeftEdgeType) &&
                    (neighbourBottomCard == null || neighbourBottomCard.TopEdgeType == card.BottomEdgeType) &&
                    (neighbourRightCard == null || neighbourRightCard.LeftEdgeType == card.RightEdgeType))
                {
                    return true;
                }
            }

            return false;
        }
    }
}