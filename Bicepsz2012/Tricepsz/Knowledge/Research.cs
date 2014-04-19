using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tricepsz.Knowledge
{
    public class Research : CivSharp.Common.ResearchData
    {
        public const Research VILLAGE = null;//= new Research(ResearchType.VILLAGE, 200, null);
        public const Research TOWN = null;// = new Research(ResearchType.TOWN, 300, VILLAGE);
        public const Research TOWER = null;// = new Research(ResearchType.TOWER, 100, null);
        public const Research BLACKSMITH = null;// = new Research(ResearchType.BLACKSMITH, 150, null);
        public const Research BARRACKS = null;// = new Research(ResearchType.BARRACKS, 200, null);
        public const Research ACADEMY = null;// = new Research(ResearchType.ACADEMY, 500, BARRACKS);
        public const Research TOWNHALL = null;// = new Research(ResearchType.TOWNHALL, 150, VILLAGE);
        public const Research BANK = null;// = new Research(ResearchType.BANK, 300, TOWN);
        public const Research BARICADE = null;// = new Research(ResearchType.BARICADE, 100, VILLAGE);
        public const Research WALL = null;// = new Research(ResearchType.WALL, 200, BARICADE);

        public Research Dependency { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public ResearchType Type { get; set; }

        private Research(ResearchType type, int cost, Research dep)
        {
            Research VILLAGE = new Research(ResearchType.VILLAGE, 200, null);
            Research TOWN = new Research(ResearchType.TOWN, 300, VILLAGE);
            Research TOWER = new Research(ResearchType.TOWER, 100, null);
            Research BLACKSMITH = new Research(ResearchType.BLACKSMITH, 150, null);
            Research BARRACKS = new Research(ResearchType.BARRACKS, 200, null);
            Research ACADEMY = new Research(ResearchType.ACADEMY, 500, BARRACKS);
            Research TOWNHALL = new Research(ResearchType.TOWNHALL, 150, VILLAGE);
            Research BANK = new Research(ResearchType.BANK, 300, TOWN);
            Research BARICADE = new Research(ResearchType.BARICADE, 100, VILLAGE);
            Research WALL = new Research(ResearchType.WALL, 200, BARICADE);

            this.Type = type;
            switch (type)
            {
                case ResearchType.VILLAGE:
                    Name = "falu";
                    break;
                case ResearchType.TOWN:
                    Name = "város";
                    break;
                case ResearchType.TOWER:
                    Name = "őrzők tornya";
                    break;
                case ResearchType.BLACKSMITH:
                    Name = "kovácsműhely";
                    break;
                case ResearchType.BARRACKS:
                    Name = "barakk";
                    break;
                case ResearchType.ACADEMY:
                    Name = "harci akadémia";
                    break;
                case ResearchType.TOWNHALL:
                    Name = "városháza";
                    break;
                case ResearchType.BANK:
                    Name = "bank";
                    break;
                case ResearchType.BARICADE:
                    Name = "barikád";
                    break;
                case ResearchType.WALL:
                    Name = "fal";
                    break;
                default:
                    Name = null;
                    break;
            }
            Dependency = dep;
            Cost = cost;
            this.WhatToResearch = type.ToString();
        }

    }

    public enum ResearchType
    {
        VILLAGE,
        TOWN,
        TOWER,
        BLACKSMITH,
        BARRACKS,
        ACADEMY,
        TOWNHALL,
        BANK,
        BARICADE,
        WALL
    }
}
