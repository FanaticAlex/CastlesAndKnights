using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Carcassone.Core.Calculation
{
    public abstract class ObjectPart
    {
        private List<Side> _originalSides { get; set; } = new List<Side>();

        public Meeple? Meeple { get; set; }
        public Flag? Flag { get; set; }

        public string PartId => Tile.Id + PartName;
        public Tile Tile {  get; set; }
        public string PartName { get; set; }
        public string PartType { get; set; }

        public void AddSide(Side side)
        {
            _originalSides.Add(side);
        }

        public ObjectPart(string partName, Tile tile)
        {
            Tile = tile;
            PartName = partName;
            PartType = string.Empty;
        }

        public List<TileBorder> GetBorders()
        {
            return _originalSides
                .Select(s => GetRotatedSide(s, Tile.RotationsCount))
                .Select(s => new TileBorder(Tile.Location, s)).ToList();
        }

        private static Side GetRotatedSide(Side side, int rotationCount)
        {
            var result = side;
            for (int i = 0; i < rotationCount; i++)
                result = RotateOnce(result);

            return result;
        }

        private static Side RotateOnce(Side side)
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