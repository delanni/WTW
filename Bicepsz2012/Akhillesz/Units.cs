using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akhillesz
{
    abstract class UnitProperties
    {
        public string Name { get; set; }
        public int Cost { get; set; }
        public int Damage { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int MovementPoints { get; set; }
        public string Requires { get; set; }
    }

    static class Units
    {
        public static readonly List<UnitProperties> Types = new List<UnitProperties>
        {
            new Felderito(),
            new Orzo(),
            new Lovag(),
            new VivoTanonc(),
            new VivoMester()
        };

        private sealed class Felderito : UnitProperties
        {
            public Felderito()
            {
                Name = "felderítő";
                Cost = 50;
                Damage = 2;
                Health = 10;
                Attack = 2;
                Defense = 3;
                MovementPoints = 4;                
            }
        }

        private sealed class Orzo : UnitProperties
        {
            public Orzo()
            {
                Name = "őrző";
                Cost = 75;
                Damage = 5;
                Health = 30;
                Attack = 6;
                Defense = 10;
                MovementPoints = 1;
                Requires = "őrzők tornya";
            }
        }

        private sealed class Lovag : UnitProperties
        {
            public Lovag()
            {
                Name = "lovag";
                Cost = 150;
                Damage = 15;
                Health = 20;
                Attack = 12;
                Defense = 8;
                MovementPoints = 1;
                Requires = "kovácsműhely";
            }
        }

        private sealed class VivoTanonc : UnitProperties
        {
            public VivoTanonc()
            {
                Name = "vívó tanonc";
                Cost = 100;
                Damage = 10;
                Health = 10;
                Attack = 6;
                Defense = 6;
                MovementPoints = 2;
                Requires = "barakk";
            }
        }

        private sealed class VivoMester : UnitProperties
        {
            public VivoMester()
            {
                Name = "vívó mester";
                Cost = 200;
                Damage = 10;
                Health = 10;
                Attack = 100;
                Defense = 6;
                MovementPoints = 2;
                Requires = "harci akadémia";
            }
        }
    }    
}

