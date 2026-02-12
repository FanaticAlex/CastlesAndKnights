using Carcassone.Core.Board;
using Carcassone.Core.Calculation;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Carcassone.Core.Tiles
{
    /// <summary>
    /// Game tile. Square card represents landscape or parts of game objects.
    /// Could be placed and connected with other tiles.
    /// </summary>
    public abstract class Tile
    {
        /// <summary>
        /// Вручную соединенные замки и поля,
        /// это нужно для подсчета какие замки присоденены к полям при подсчете очков за поля
        /// </summary>
        public Dictionary<FarmPart, List<CityPart>> FarmToCityParts { get; set; } = new Dictionary<FarmPart, List<CityPart>>();

        public List<ObjectPart> Parts { get; set; } = new List<ObjectPart>();

        public string Id { get; set; }
        public string CardType { get; set; }
        public int CardNumber { get; set; }

        /// <summary>
        /// define where in a tile stack pile this card should apear
        /// </summary>
        public int StackPriority { get; set; }

        public char TopEdgeType { get; set; }
        public char RightEdgeType { get; set; }
        public char BottomEdgeType { get; set; }
        public char LeftEdgeType { get; set; }


        public int RotationsCount { get; set; }

        public Point Location { get; set; }

        public Tile(string cardType, int cardNumber)
        {
            Id = GetTileId(cardType, cardNumber);
            CardType = cardType;
            CardNumber = cardNumber;
            TopEdgeType = cardType[0];
            RightEdgeType = cardType[1];
            BottomEdgeType = cardType[2];
            LeftEdgeType = cardType[3];
        }

        public static string GetTileId(string cardType, int cardNumber) => $"{cardType}({cardNumber})";

        public ObjectPart GetPart(string partName)
        {
            var list = Parts.Where(p => p.PartName == partName);
            if (list.Count() == 0) throw new NullReferenceException("Part not found: " + partName);

            if (list.Count() > 1) throw new InvalidOperationException("Не может быть два обьекта с одним названием");

            return list.Single();
        }

        public List<CityPart> GetConnectedCityParts(FarmPart part)
        {
            var CityParts = new List<CityPart>();
            if (FarmToCityParts.Keys.Contains(part))
            {
                CityParts = FarmToCityParts[part];
            }

            return CityParts;
        }

        public void RotateCard(int rotation)
        {
            if (rotation < 0) throw new ArgumentOutOfRangeException("неверное число поворотов");
            if (rotation >= 4) throw new ArgumentOutOfRangeException("неверное число поворотов");

            do
                RotateTile();
            while (RotationsCount != rotation);
        }

        /// <summary>
        /// Rotate card at 90 degree clockwise. Rotation counter increases by 1
        /// </summary>
        public void RotateTile()
        {
            var tempTop = TopEdgeType;
            var tempLeft = LeftEdgeType;
            var tempBottom = BottomEdgeType;
            var tempRight = RightEdgeType;

            TopEdgeType = tempLeft;
            LeftEdgeType = tempBottom;
            BottomEdgeType = tempRight;
            RightEdgeType = tempTop;

            RotationsCount += 1;
            RotationsCount %= 4;

            // rotate parts of object on a tile as well
            foreach(var part in Parts)
            {
                for (int i = 0; i < part.Sides.Count; i++)
                    part.Sides[i] = GetRotatedSide(part.Sides[i], 1);
            }
        }

        private static Side GetRotatedSide(Side side, int rotationCount)
        {
            switch (side)
            {
                case Side.top: return Side.right;
                case Side.right: return Side.bottom;
                case Side.bottom: return Side.left;
                case Side.left: return Side.top;
                case Side.side_0: return Side.side_2;
                case Side.side_1: return Side.side_3;
                case Side.side_2: return Side.side_4;
                case Side.side_3: return Side.side_5;
                case Side.side_4: return Side.side_6;
                case Side.side_5: return Side.side_7;
                case Side.side_6: return Side.side_0;
                case Side.side_7: return Side.side_1;
                default: throw new NotImplementedException();
            }
        }
    }
}