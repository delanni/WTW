using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CivSharp;

namespace CivSharp.Bicepsz
{
    public static class UnitStats
    {
        public static readonly Dictionary<string, int> AttackPower = new Dictionary<string,int>()
        {
            {Units.Talpas,1},
            {Units.Ijasz,1},
            {Units.Landzsas,2},
            {Units.Lovag,4},
            {Units.Katapult,8},
        };

        public static readonly Dictionary<string, int> DefensePower = new Dictionary<string, int>()
        {
            {Units.Talpas,1},
            {Units.Ijasz,3},
            {Units.Landzsas,2},
            {Units.Lovag,4},
            {Units.Katapult,3},
        };
        public static readonly Dictionary<string, int> TotalMoves = new Dictionary<string, int>()
        {
            {Units.Talpas,2},
            {Units.Ijasz,2},
            {Units.Landzsas,2},
            {Units.Lovag,3},
            {Units.Katapult,1},
        };
    }
}
