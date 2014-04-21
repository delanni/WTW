using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tricepsz.Knowledge
{
    public class Research
    {
        public static Research VILLAGE = null;//= new Research(ResearchType.VILLAGE, 200, null);
        public static Research TOWN = null;// = new Research(ResearchType.TOWN, 300, VILLAGE);
        public static Research TOWER = null;// = new Research(ResearchType.TOWER, 100, null);
        public static Research BLACKSMITH = null;// = new Research(ResearchType.BLACKSMITH, 150, null);
        public static Research BARRACKS = null;// = new Research(ResearchType.BARRACKS, 200, null);
        public static Research ACADEMY = null;// = new Research(ResearchType.ACADEMY, 500, BARRACKS);
        public static Research TOWNHALL = null;// = new Research(ResearchType.TOWNHALL, 150, VILLAGE);
        public static Research BANK = null;// = new Research(ResearchType.BANK, 300, TOWN);
        public static Research BARICADE = null;// = new Research(ResearchType.BARICADE, 100, VILLAGE);
        public static Research WALL = null;// = new Research(ResearchType.WALL, 200, BARICADE);

        public Research Dependency { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public ResearchType Type { get; set; }

        public static void Initialize()
        {
            VILLAGE = new Research(ResearchType.VILLAGE, 200, null);
            TOWN = new Research(ResearchType.TOWN, 300, VILLAGE);
            TOWER = new Research(ResearchType.TOWER, 100, null);
            BLACKSMITH = new Research(ResearchType.BLACKSMITH, 150, null);
            BARRACKS = new Research(ResearchType.BARRACKS, 200, null);
            ACADEMY = new Research(ResearchType.ACADEMY, 500, BARRACKS);
            TOWNHALL = new Research(ResearchType.TOWNHALL, 150, VILLAGE);
            BANK = new Research(ResearchType.BANK, 300, TOWN);
            BARICADE = new Research(ResearchType.BARICADE, 100, VILLAGE);
            WALL = new Research(ResearchType.WALL, 200, BARICADE);
        }

        private Research(ResearchType type, int cost, Research dep)
        {

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
        }


        internal static Research FromName(string name)
        {
            if (name == "fal") return Research.WALL;
            if (name == "falu") return Research.VILLAGE;
            if (name == "város") return Research.TOWN;
            if (name == "őrzők tornya") return Research.TOWER;
            if (name == "kovácsműhely") return Research.BLACKSMITH;
            if (name == "barakk") return Research.BARRACKS;
            if (name == "harci akadémia") return Research.ACADEMY;
            if (name == "városháza") return Research.TOWNHALL;
            if (name == "bank") return Research.BANK;
            if (name == "barikád") return Research.BARICADE;
            return null;
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
