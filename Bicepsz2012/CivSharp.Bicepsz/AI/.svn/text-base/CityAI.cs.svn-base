using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CivSharp.Common;
using System.Diagnostics;

namespace CivSharp.Bicepsz
{
    public class CityAI
    {
        private CityInfo cityInfo;
        private HiveMind hiveMind;

        public CityInfo CityInfo { get { return this.cityInfo; } }

        public CityAI(CityInfo cityInfo, HiveMind hiveMind)
        {
            this.cityInfo = cityInfo;
            this.hiveMind = hiveMind;
        }

        public TrainCommand OnTrain()
        {
            var targetUnit = BuyHelper.CanBuy(hiveMind.World, Units.Ijasz) ? Units.Ijasz : Units.Talpas;
            if (this.hiveMind.MyPlayer.Money < 50)
                return null;

            TrainCommand trainCommand;
            int attackPower = hiveMind.PossibleAttackPowers[cityInfo.PositionX, cityInfo.PositionY];
            double defensePower = hiveMind.DefensePowers[cityInfo.PositionX, cityInfo.PositionY];
            // ha a lehetseges beerkezo tamadoero osszege nagyobb mint az aktualis vedelem
            if (defensePower < attackPower)
            {
                double defenseCoeff = 1;
                if (hiveMind.World.GetMyPlayer().Researched.Contains(Research.kofal)) defenseCoeff = 1.4;
                else if (hiveMind.World.GetMyPlayer().Researched.Contains(Research.colopkerites)) defenseCoeff = 1.2;
                int count = (int)Math.Ceiling((attackPower - defensePower) / (UnitStats.DefensePower[Units.Ijasz] * defenseCoeff));
                return trainCommand = new TrainCommand()
                {
                    Count = count,
                    Unit = targetUnit,
                };
            }
            else if (this.hiveMind.DomainBorder[this.cityInfo.PositionX, this.cityInfo.PositionY] &&
                (this.cityInfo.PositionY + this.cityInfo.PositionX) % 2 == 0 &&
                !this.hiveMind.World.Units.Any(w => w.PositionX == this.cityInfo.PositionX && w.PositionY == this.cityInfo.PositionY) &&
                hiveMind.World.GetMyUnits().Count() <= hiveMind.World.GetIncome() / 140) // van elerheto tavolsagon belul terjeszkedesi pont es nincs ott egyseg es  && this.myPlayer.Money >= 50
            {
                return new TrainCommand()
                    {
                        Count = 1,
                        Unit = targetUnit,
                    };
            }
            return null;
        }

        #region TrainCommand class

        public class TrainCommand
        {
            public string Unit { get; set; }
            public int Count { get; set; }
        }

        #endregion
    }
}
