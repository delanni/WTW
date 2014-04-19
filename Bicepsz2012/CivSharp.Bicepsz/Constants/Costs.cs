using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CivSharp.Bicepsz
{
    public static class Costs
    {
        public static readonly Dictionary<string, int> ResearchCost = new Dictionary<string, int>()
        {
            {Research.famegmunkalas,50},
            {Research.landzsa,70},
            {Research.ijaszat,50},
            {Research.kerek,200},
            {Research.katapult,300},
            {Research.lovag,150},
            {Research.iras,100},
            {Research.birosag,300},
            {Research.colopkerites,100},
            {Research.kofal,300},
        };

        public static readonly Dictionary<string, int> UnitCost = new Dictionary<string, int>()
        {
            {Units.Talpas, 50},
            {Units.Ijasz, 50},
            {Units.Landzsas, 50},
            {Units.Lovag, 100},
            {Units.Katapult,200}
        };
    }
}
