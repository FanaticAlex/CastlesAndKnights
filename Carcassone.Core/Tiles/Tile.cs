using Carcassone.Core.Calculation;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;
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
        public List<ObjectPart> Parts { get; set; } = new List<ObjectPart>();

        public string Id { get; set; } // tileType_tileNumber
        public string TileType { get; set; }
        public int TileNumber { get; set; }

        /// <summary>
        /// define where in a tile stack pile this card should apear
        /// </summary>
        public int StackPriority { get; set; }

        public char TopEdgeType => GetEdge(Side.top, RotationsCount);
        public char RightEdgeType => GetEdge(Side.right, RotationsCount);
        public char BottomEdgeType => GetEdge(Side.bottom, RotationsCount);
        public char LeftEdgeType => GetEdge(Side.left, RotationsCount);

        public Point Location { get; set; }

        public int RotationsCount { get; set; } // {0,1,2,3}

        public Tile(string _tileType, int _tileNumber)
        {
            Id = GetTileId(_tileType, _tileNumber);
            TileType = _tileType;
            TileNumber = _tileNumber;
        }

        public static string GetTileId(string cardType, int cardNumber) => $"{cardType}({cardNumber})";

        public ObjectPart GetPart(string partName)
        {
            var list = Parts.Where(p => p.PartName == partName);
            if (list.Count() == 0) throw new NullReferenceException("Part not found: " + partName);

            if (list.Count() > 1) throw new InvalidOperationException("Не может быть два обьекта с одним названием");

            return list.Single();
        }

        public void RotateTileTo(int rotation)
        {
            if (rotation < 0) throw new ArgumentOutOfRangeException("неверное число поворотов");
            if (rotation > 3) throw new ArgumentOutOfRangeException("неверное число поворотов");

            do
                RotateTile();
            while (RotationsCount != rotation);
        }

        /// <summary>
        /// Rotate card at 90 degree clockwise. Rotation counter increases by 1
        /// </summary>
        public void RotateTile()
        {
            RotationsCount += 1;
            RotationsCount %= 4;
        }

        private char GetEdge(Side side, int rotationCoun)
        {
            var rotatedSide = side;
            for (int i = 0; i < rotationCoun; i++)
            {
                rotatedSide = RotateOnce(rotatedSide);
            }
                
            switch (rotatedSide)
            {
                case Side.top: return TileType[0];
                case Side.right: return TileType[1];
                case Side.bottom: return TileType[2];
                case Side.left: return TileType[3];
            }
            
            throw new ArgumentException();
        }

        private Side RotateOnce(Side side)
        {
            switch (side)
            {
                case Side.top: return Side.left;
                case Side.right: return Side.top;
                case Side.bottom: return Side.right;
                case Side.left: return Side.bottom;
            }

            throw new ArgumentException();
        }
    }
}