namespace Carcassone.Core.Tiles
{
    /// <summary>
    /// top, right, left, bottom for cities, roads, ..
    /// 
    /// side_0 - side_7 for fields devided by road or river
    /// </summary>
    public enum Side
    {
        //           top
        //        |       | 
        //  left  |       | right
        //        |       | 
        //          bottom 
        top,
        right,
        bottom,
        left,

        //      7   0
        //  6 |       | 1
        //    |       | 
        //  5 |       | 2
        //      4   3
        side_0,
        side_1,
        side_2,
        side_3,
        side_4,
        side_5,
        side_6,
        side_7,
    };
}