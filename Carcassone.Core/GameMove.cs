using System;
using System.Collections.Generic;
using System.Drawing;

namespace Carcassone.Core
{
    public class MoveComparer : IEqualityComparer<GameMove>
    {
        public bool Equals(GameMove x, GameMove y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.Location == y.Location &&
                   x.TileRotation == y.TileRotation &&
                   x.TileId == y.TileId &&
                   ((x.PartName == y.PartName) || (string.IsNullOrEmpty(x.PartName) && string.IsNullOrEmpty(y.PartName))) &&
                   x.PlayerName == y.PlayerName;
        }

        public int GetHashCode(GameMove b)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(b, null)) return 0;

            //Get hash code for the Name field if it is not null.
            var str = b.Location.ToString() + b.TileRotation + b.TileId + b.PartName + b.PlayerName;
            int code = str.GetHashCode();

            return code;
        }
    }
    public class GameMove
    {
        public string? PlayerName { get; set; }
        public string TileId { get; set; }

        private int _tileRotation;
        public int TileRotation
        {
            get { return _tileRotation; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("неверное число поворотов");
                if (value > 3) throw new ArgumentOutOfRangeException("неверное число поворотов");

                _tileRotation = value;
            }
        }

        public Point Location { get; set; }

        public string? PartName { get; set; }
    }
}
