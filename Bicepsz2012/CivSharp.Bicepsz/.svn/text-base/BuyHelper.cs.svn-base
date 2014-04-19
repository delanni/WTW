using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CivSharp.Common;

namespace CivSharp.Bicepsz
{
    public static class BuyHelper
    {
        public static bool CanBuy(this WorldInfo world, string unitName, Point position =null)
        {
            if (world.GetMyPlayer().Money > Costs.UnitCost[unitName] 
                && 
                (Dependencies.UnitDependency[unitName] == string.Empty 
                || world.GetMyPlayer().Researched.Contains(Dependencies.UnitDependency[unitName])))
                return true;

            else return false;
        }

        public static TrainingData Buy(this WorldInfo world, string unitName, Point position)
        {
            var point =  world.GetNearestCityTo(position);
            world.GetMyPlayer().Money -= Costs.UnitCost[unitName];
            return new TrainingData() { PositionX = point.PositionX, PositionY = point.PositionY, UnitTypeName = unitName };
        }
    }
}
