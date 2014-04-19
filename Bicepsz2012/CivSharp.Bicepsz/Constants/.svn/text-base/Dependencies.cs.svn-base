using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CivSharp.Bicepsz
{
    public static class Dependencies
    {
        public static readonly Dictionary<string, string> ResearchDependency = new Dictionary<string, string>()
        {
            {Research.katapult, Research.kerek},
            {Research.kerek, Research.famegmunkalas},
            {Research.ijaszat, Research.famegmunkalas},
            {Research.landzsa, Research.famegmunkalas},
            {Research.lovag, Research.lotenyesztes},
            {Research.birosag, Research.iras},
            {Research.kofal, Research.colopkerites},
            {Research.famegmunkalas, string.Empty},
            {Research.iras, string.Empty},
            {Research.colopkerites, string.Empty},
            {Research.lotenyesztes, string.Empty}
        };

        public static readonly Dictionary<string, string> UnitDependency = new Dictionary<string, string>()
        {
            {Units.Talpas, string.Empty},
            {Units.Ijasz, Research.ijaszat},
            {Units.Landzsas, Research.landzsa},
            {Units.Lovag, Research.lovag},
            {Units.Katapult, Research.katapult}
        };
    }
}
