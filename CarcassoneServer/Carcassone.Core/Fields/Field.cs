using System;
using Carcassone.Core.Cards;
using Newtonsoft.Json;

namespace Carcassone.Core.Fields
{

    /// <summary>
    /// Игровое поле может содержать карту и фишку.
    /// </summary>
    public class Field
    {
        public Field(int x, int y)
        {
            Id = $"{x}_{y}";
            X = x;
            Y = y;
        }

        public string CardName { get; set; }

        /// <summary>
        /// Координаты поля на игровой сетке
        /// </summary>
        public int X { get; }
        public int Y { get; }

        public static bool operator ==(Field? a, Field? b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return (a.X == b.X && a.Y == b.Y);
        }

        public static bool operator !=(Field? a, Field? b) => !(a == b);

        public string Id { get; }

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

            // если есть граничные карты то границы должны совпадать иначе карту присоединить нельзя
            var isRiverCard = card.CardId.Contains("W");
            if (isRiverCard)
            {
                Card? neighbourTopCard = cardPool.GetCard(fieldBoard.GetNeighbour(this, Side.top)?.CardName);
                Card? neighbourLeftCard = cardPool.GetCard(fieldBoard.GetNeighbour(this, Side.left)?.CardName);
                Card? neighbourBottomCard = cardPool.GetCard(fieldBoard.GetNeighbour(this, Side.bottom)?.CardName);
                Card? neighbourRightCard = cardPool.GetCard(fieldBoard.GetNeighbour(this, Side.right)?.CardName);

                bool isTopFree = neighbourTopCard == null;
                bool isLeftFree = neighbourLeftCard == null;
                bool isBottomFree = neighbourBottomCard == null;
                bool isRightFree = neighbourRightCard == null;

                // направелние реки должно быть строго сверху вниз, поворачивать реку наверх нельзя
                bool isWaterDirectionCorrect = !(isTopFree && card.TopEdgeType == EdgeType.Water);

                bool connectWithTopWaterCard = (neighbourTopCard?.BottomEdgeType == card.TopEdgeType && card.TopEdgeType == EdgeType.Water);
                bool connectWithLeftWaterCard = (neighbourLeftCard?.RightEdgeType == card.LeftEdgeType && card.LeftEdgeType == EdgeType.Water);
                bool connectWithBottomWaterCard = (neighbourBottomCard?.TopEdgeType == card.BottomEdgeType && card.BottomEdgeType == EdgeType.Water);
                bool connectWithRightWaterCard = (neighbourRightCard?.LeftEdgeType == card.RightEdgeType && card.RightEdgeType == EdgeType.Water);

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
                Card? neighbourTopCard = cardPool.GetCard(fieldBoard.GetNeighbour(this, Side.top)?.CardName);
                Card? neighbourLeftCard = cardPool.GetCard(fieldBoard.GetNeighbour(this, Side.left)?.CardName);
                Card? neighbourBottomCard = cardPool.GetCard(fieldBoard.GetNeighbour(this, Side.bottom)?.CardName);
                Card? neighbourRightCard = cardPool.GetCard(fieldBoard.GetNeighbour(this, Side.right)?.CardName);
                
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