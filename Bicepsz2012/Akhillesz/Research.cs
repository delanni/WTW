using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akhillesz
{
    class Research
    {
        public string Name { get; set; }
        public int Cost { get; set; }
        public double Defense { get; set; }
        public int AdditionalIncome { get; set; }
        public string Requires { get; set; }
    }

    static class AvailableResearch {

        public static readonly List<Research> Choices = new List<Research>
        {
            new Falu(),
            new Varos() { Requires = "falu" },
            new OrzokTornya(),
            new Kovacsmuhely(),
            new Barakk(),
            new HarciAkademia() { Requires = "barakk" },
            new VarosHaza() { Requires = "falu" },
            new Bank() { Requires = "város" },
            new Barikad() { Requires = "falu" },
            new Fal() { Requires = "barikád" }
        };

        private sealed class Falu : Research
        {
            public Falu()
            {
                Name = "falu";
                Cost = 200;
                Defense = 0;
                AdditionalIncome = 0;
            }
        }

        private sealed class Varos : Research
        {
            public Varos()
            {
                Name = "város";
                Cost = 300;
                Defense = 0;
                AdditionalIncome = 0;                
            }
        }

        private sealed class OrzokTornya : Research
        {
            public OrzokTornya()
            {
                Name = "őrzők tornya";
                Cost = 100;
                Defense = 0;
                AdditionalIncome = 0;
            }
        }

        private sealed class Kovacsmuhely : Research
        {
            public Kovacsmuhely()
            {
                Name = "kovácsműhely";
                Cost = 150;
                Defense = 0;
                AdditionalIncome = 0;
            }
        }

        private sealed class Barakk : Research
        {
            public Barakk()
            {
                Name = "barakk";
                Cost = 200;
                Defense = 0;
                AdditionalIncome = 0;
            }
        }

        private sealed class HarciAkademia : Research
        {
            public HarciAkademia()
            {
                Name = "harci akadémia";
                Cost = 500;
                Defense = 0;
                AdditionalIncome = 0;
            }
        }

        private sealed class VarosHaza : Research
        {
            public VarosHaza()
            {
                Name = "városháza";
                Cost = 150;
                Defense = 0;
                AdditionalIncome = 10;
            }
        }

        private sealed class Bank : Research
        {
            public Bank()
            {
                Name = "bank";
                Cost = 300;
                Defense = 0;
                AdditionalIncome = 20;
            }
        }

        private sealed class Barikad : Research
        {
            public Barikad()
            {
                Name = "barikád";
                Cost = 100;
                Defense = .20;
                AdditionalIncome = 0;
            }
        }

        private sealed class Fal : Research
        {
            public Fal()
            {
                Name = "fal";
                Cost = 200;
                Defense = .30;
                AdditionalIncome = 0;
            }
        }
    }


}
