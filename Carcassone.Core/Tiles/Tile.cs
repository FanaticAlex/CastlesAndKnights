using Carcassone.Core.Calculation;
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

        public static char GetEdge(string tileType, Side side, int rotationCoun)
        {
            var rotatedSide = side;
            for (int i = 0; i < rotationCoun; i++)
            {
                rotatedSide = RotateOnce(rotatedSide);
            }
                
            switch (rotatedSide)
            {
                case Side.top: return tileType[0];
                case Side.right: return tileType[1];
                case Side.bottom: return tileType[2];
                case Side.left: return tileType[3];
            }
            
            throw new ArgumentException();
        }

        private static Side RotateOnce(Side side)
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