using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CivSharp.Bicepsz
{
    public class DomainMap
    {
        public CellState[,] MapBase { get; private set; }

        public DomainMap()
        {
            MapBase = new CellState[15, 15];
            ClearMap();
        }

        public void ClearMap()
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    MapBase[i, j] = CellState.Safe;
                }
            }
        }

        public void SetCell(Point p, CellState state)
        {
            MapBase[p.X, p.Y] = state;
        }
        public CellState GetCell(Point p)
        {
            return MapBase[p.X, p.Y];
        }
    }
    public enum CellState
    {
        Safe,
        Danger,
        Urgent,
    }
}
