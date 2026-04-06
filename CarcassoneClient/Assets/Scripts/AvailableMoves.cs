using Carcassone.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;

namespace Assets.Scripts
{
    public class AvailableMoves
    {
        public List<GameMove> _moves = new List<GameMove>();

        public AvailableMoves(List<GameMove> moves)
        {
            _moves = moves;
        }

        public bool CanPut(Point cell)
        {
            return _moves.Select(m => m.Location).Distinct().Contains(cell);
        }

        public IEnumerable<int> GetAvailableRotations(Point cell) 
        {
            return _moves
                .Where(m => m.Location == cell)
                .Select(m => m.TileRotation)
                .Distinct();
        }

        public IEnumerable<GameMove> GetMoves(Point cell, int rotation)
        {
            return _moves
                .Where(m => m.Location == cell)
                .Where(m => m.TileRotation == rotation);
        }

        public IEnumerable<Point> GetLocations()
        {
            return _moves.Select(m => m.Location).Distinct();
        }
    }
}
