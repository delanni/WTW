using CivSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tricepsz.Knowledge;

namespace Tricepsz.Helpers
{
    public static class ConversionHelpers
    {
        public static IEnumerable<Unit> ToUnits(this UnitInfo[] units)
        {
            foreach (var unit in units)
            {
                yield return new Unit(unit);
            }
        }
    }
}
