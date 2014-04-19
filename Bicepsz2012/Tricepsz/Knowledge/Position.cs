using CivSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tricepsz.Knowledge
{
    class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public double DistanceTo(Position other)
        {
            int dx = other.X - X;
            int dy = other.Y - Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public int StepsTo(Position other)
        {
            int dx = other.X - X;
            int dy = other.Y - Y;
            return Math.Abs(dx) + Math.Abs(dy);
        }

    }
}
