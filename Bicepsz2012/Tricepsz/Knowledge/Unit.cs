using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tricepsz.Knowledge
{
    public class Unit
    {
        public static Unit SCOUT = new Unit(UnitType.SCOUT);

        private UnitType _type;
        public UnitType Type
        {
            get
            {
                return _type;
            }
            set
            {
                this._type = value;
                switch (value)
                {
                    case UnitType.SCOUT:
                        _setStats(50, 2, 10, 2, 3, 4, null, value);
                        break;
                    case UnitType.GUARD:
                        _setStats(75, 5, 30, 6, 10, 1, Research.TOWER,value);
                        break;
                    case UnitType.KNIGHT:
                        _setStats(150, 15, 20, 12, 8, 1, Research.BLACKSMITH,value);
                        break;
                    case UnitType.CHAMPION:
                        _setStats(100, 10, 10, 6, 6, 2, Research.BARRACKS,value);
                        break;
                    case UnitType.KNIGHTCHAMPION:
                        _setStats(200, 10, 10, 100, 6, 2, Research.ACADEMY,value);
                        break;
                    default:
                        break;
                }
            }
        }
        public Research Dependency { get; set; }
        public bool CanBuild { get { return Dependency == null; } }
        public int HitPoints { get; set; }
        public int MovementPoints { get; set; }
        public string Owner { get; set; }
        public string UnitId { get; set; }
        private string _unitTypeName;
        public string UnitTypeName { get {
            return _unitTypeName;
        }
            set {
                _unitTypeName = value;
                switch (value)
                {
                    case "felderítő":
                        Type = UnitType.SCOUT;
                        break;
                    case "őrző":
                        Type = UnitType.GUARD;
                        break;
                    case "lovag":
                        Type = UnitType.KNIGHT;
                        break;
                    case "vívó tanonc":
                        Type = UnitType.CHAMPION ;
                        break;
                    case "vívó mester":
                        Type = UnitType.KNIGHTCHAMPION;
                        break;
                    default:
                        Type = UnitType.SCOUT;
                        break;
                }
            }
        }
        internal Position Position { get; set; }
        public int ATK { get; set; }
        public int DEF { get; set; }
        public int HP { get; set; }
        public int DMG { get; set; }
        public int SPD { get; set; }
        public int COST { get; set; }

        public Unit(CivSharp.Common.UnitInfo x)
        {
            this.UnitTypeName = x.UnitTypeName;
            this.UnitId = x.UnitID;
            this.Position = new Position(x.PositionX, x.PositionY);
            this.Owner = x.Owner;
            this.MovementPoints = x.MovementPoints;
            this.HitPoints = x.HitPoints;
        }

        public Unit(UnitType unitType)
        {
            this.Type = unitType;
        }

        private void _setStats(int cost, int dmg, int hp, int atk, int def, int spd, Research dep, UnitType type)
        {
            this._type = type;
            this.COST = cost;
            this.DMG = dmg;
            this.HP = hp;
            this.ATK = atk;
            this.DEF = def;
            this.SPD = spd;
            this.Dependency = dep;

            switch (type)
            {
                case UnitType.SCOUT:
                    this._unitTypeName = "felderítő";
                    break;
                case UnitType.GUARD:
                    this._unitTypeName = "őrző";
                    break;
                case UnitType.KNIGHT:
                    this._unitTypeName = "lovag";
                    break;
                case UnitType.CHAMPION:
                    this._unitTypeName = "vívó tanonc";
                    break;
                case UnitType.KNIGHTCHAMPION:
                    this._unitTypeName = "vívó mester";
                    break;
            }
        }

        internal static string NameFromType(UnitType type)
        {
            switch (type)
            {
                case UnitType.SCOUT:
                    return "felderítő";
                    break;
                case UnitType.GUARD:
                    return "őrző";
                    break;
                case UnitType.KNIGHT:
                    return "lovag";
                    break;
                case UnitType.CHAMPION:
                    return "vívó tanonc";
                    break;
                case UnitType.KNIGHTCHAMPION:
                    return "vívó mester";
                    break;
                default:
                    return "felderítő";
            }
        }
    }

    public enum UnitType
    {
        SCOUT,
        GUARD,
        KNIGHT,
        CHAMPION,
        KNIGHTCHAMPION
    }
}
