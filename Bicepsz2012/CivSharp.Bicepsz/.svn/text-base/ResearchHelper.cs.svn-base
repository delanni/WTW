using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CivSharp.Common;

namespace CivSharp.Bicepsz
{
    static class ResearchHelper
    {
        public static ResearchData Upgrade(this PlayerInfo player, string upgrade)
        {
            player.Money -= Costs.ResearchCost[upgrade];
            player.Researched = player.Researched.Concat(new string[] { upgrade }).ToArray();
            return new ResearchData() { WhatToResearch = upgrade };
        }

        public static bool CanResearch(this PlayerInfo player, string upgrade)
        {
            if (!player.Researched.Contains(upgrade) && player.Money >= Costs.ResearchCost[upgrade])
            {
                //megvan rá a pénz, és még nincs kifejlesztve
                if (player.Researched.Contains(Dependencies.ResearchDependency[upgrade]) || Dependencies.ResearchDependency[upgrade] == string.Empty)
                    return true;
                else return false;
            }
            else
                return false;

        }
    }
}
