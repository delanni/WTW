using CivSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tricepsz.Helpers;

namespace Tricepsz.Knowledge
{
    public class Map
    {
        public Map(WorldInfo world)
        {
            this.Cities = world.Cities;
            this.Players = world.Players;
            this.Units = world.Units.ToUnits();
        }



        public CityInfo[] Cities { get; set; }

        public PlayerInfo[] Players { get; set; }

        public IEnumerable<Unit> Units { get; set; }
    }
}
