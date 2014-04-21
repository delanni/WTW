using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akhillesz
{
    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public double DistanceFrom(int x, int y)
        {
            return Math.Sqrt( Math.Abs( X - x ) + Math.Abs(Y - y) );
        }
    }
}

