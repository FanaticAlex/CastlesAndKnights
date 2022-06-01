using System.Collections;
using System.Collections.Generic;
using System;
using Carcassone.Core.Cards;
using System.Dynamic;

namespace Carcassone.Core.Fields
{

    /// <summary>
    /// Игровое поле может содержать карту и фишку.
    /// </summary>
    public class Field
    {
        private FieldBoard _fieldBoard;
        private Card? _cardInField;

        public Field(FieldBoard fieldBoard, int x, int y)
        {
            Id = $"{x}_{y}";
            X = x;
            Y = y;

            _fieldBoard = fieldBoard;
        }

        public Card GetCardInField()
        {
            return _cardInField;
        }

        public void SetCardInField(Card card)
        {
            _cardInField = card;
        }

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

        public Field? GetNeighbour(Side side)
        {
            switch (side)
            {
                case Side.top:      return _fieldBoard?.GetField(X, Y + 1);
                case Side.bottom:   return _fieldBoard?.GetField(X, Y - 1);
                case Side.right:    return _fieldBoard?.GetField(X + 1, Y);
                case Side.left:     return _fieldBoard?.GetField(X - 1, Y);
                default: return null;
            }
        }

        public bool RotateCardTilFit(Card card)
        {
            for (int i = 0; i < 4; i++)
            {
                if (CanPutCardInThisField(card))
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

        internal bool CanPutCardInThisFieldWithRotation(Card? card)
        {
            if (_cardInField != null)
                return false;

            if (card == null)
                return false;

            var type = card.GetType();
            var copy = (Card)Activator.CreateInstance(type, card.CardName);
            copy.TopEdgeType = card.TopEdgeType;
            copy.LeftEdgeType = card.LeftEdgeType;
            copy.BottomEdgeType = card.BottomEdgeType;
            copy.RightEdgeType = card.RightEdgeType;

            return RotateCardTilFit(copy);
        }

        private bool IsWaterCardNear(Field? field)
        {
            if (field == null)
                return false;

            var top = field.GetNeighbour(Side.top);
            var left = field.GetNeighbour(Side.left);
            var bottom = field.GetNeighbour(Side.bottom);
            var right = field.GetNeighbour(Side.right);

            Card? TopCard = top?._cardInField;
            Card? leftCard = left?._cardInField;
            Card? bottomCard = bottom?._cardInField;
            Card? rightCard = right?._cardInField;

            if (((TopCard != null) && TopCard.CardName.Contains("W")) ||
                ((leftCard != null) && leftCard.CardName.Contains("W")) ||
                ((bottomCard != null) && bottomCard.CardName.Contains("W")) ||
                ((rightCard != null) && rightCard.CardName.Contains("W")))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Можно ли класть карту в поле
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        internal bool CanPutCardInThisField(Card card)
        {
            // если есть граничные карты то границы должны совпадать иначе карту присоединить нельзя
            var isRiverCard = card.CardName.Contains("W");
            if (isRiverCard)
            {
                Card? neighbourTopCard = GetNeighbour(Side.top)?._cardInField;
                Card? neighbourLeftCard = GetNeighbour(Side.left)?._cardInField;
                Card? neighbourBottomCard = GetNeighbour(Side.bottom)?._cardInField;
                Card? neighbourRightCard = GetNeighbour(Side.right)?._cardInField;

                bool isTopNearWater = (neighbourTopCard == null && !IsWaterCardNear(GetNeighbour(Side.top)));
                bool isLeftNearWater = (neighbourLeftCard == null && !IsWaterCardNear(GetNeighbour(Side.left)));
                bool isBottomNearWater = (neighbourBottomCard == null && !IsWaterCardNear(GetNeighbour(Side.bottom)));
                bool isRightNearWater = (neighbourRightCard == null && !IsWaterCardNear(GetNeighbour(Side.right)));

                bool topWithoutWaterCards = card.TopEdgeType != EdgeType.Water;
                bool leftWithoutWaterCards = card.LeftEdgeType != EdgeType.Water;
                bool bottmWithoutWaterCards = card.BottomEdgeType != EdgeType.Water;
                bool rightWithoutWaterCards = card.RightEdgeType != EdgeType.Water;

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
                if ((/*topWithoutWaterCards || isTopNearWater ||*/ isTopFree || connectWithTopWaterCard) &&
                    (/*leftWithoutWaterCards || isLeftNearWater ||*/ isLeftFree || connectWithLeftWaterCard) &&
                    (/*bottmWithoutWaterCards || isBottomNearWater ||*/ isBottomFree || connectWithBottomWaterCard) &&
                    (/*rightWithoutWaterCards || isRightNearWater ||*/ isRightFree || connectWithRightWaterCard) &&
                    isWaterDirectionCorrect)
                {
                    return true;
                }
            }
            else
            {
                Card? neighbourTopCard = GetNeighbour(Side.top)?._cardInField;
                Card? neighbourLeftCard = GetNeighbour(Side.left)?._cardInField;
                Card? neighbourBottomCard = GetNeighbour(Side.bottom)?._cardInField;
                Card? neighbourRightCard = GetNeighbour(Side.right)?._cardInField;
                
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