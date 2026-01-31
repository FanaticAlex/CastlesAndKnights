using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Tiles;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Carcassone.Core.Calculation
{
    public class TileBorder
    {
        public PointF Location { get; set; }

        public TileBorder(Cell cell, Side side)
        {
            SizeF size = new SizeF(0, 0);
            switch (side)
            {
                case Side.top:    size = new SizeF( 0   ,  0.5f); break;
                case Side.right:  size = new SizeF( 0.5f,  0); break;
                case Side.bottom: size = new SizeF( 0   , -0.5f); break;
                case Side.left:   size = new SizeF(-0.5f,  0); break;

                case Side.side_0: size = new SizeF( 0.25f,  0.5f); break;
                case Side.side_1: size = new SizeF( 0.5f ,  0.25f); break;
                case Side.side_2: size = new SizeF( 0.5f , -0.25f); break;
                case Side.side_3: size = new SizeF( 0.25f, -0.5f); break;
                case Side.side_4: size = new SizeF(-0.25f, -0.5f); break;
                case Side.side_5: size = new SizeF(-0.5f , -0.25f); break;
                case Side.side_6: size = new SizeF(-0.5f ,  0.25f); break;
                case Side.side_7: size = new SizeF(-0.25f , 0.5f); break;
            }

            Location = PointF.Add(cell.Location, size);
        }

        public static bool Equial(TileBorder border1, TileBorder border2)
        {
            if (border1.Location == border2.Location)
            {
                return true;
            }

            return false;
        }
    }

    public class BorderComparer : IEqualityComparer<TileBorder>
    {
        public bool Equals(TileBorder x, TileBorder y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.Location == y.Location;
        }

        public int GetHashCode(TileBorder b)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(b, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int code = b.Location == null ? 0 : b.Location.GetHashCode();

            return code;
        }
    }
}