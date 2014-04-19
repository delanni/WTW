using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CivSharp.Common;
using System.Diagnostics;

namespace CivSharp.Bicepsz
{
    public class Bicepsz : IPlayer
    {
        private Point startPoint;
        private HiveMind hiveMind;
        private List<UnitAI> units = new List<UnitAI>();
        private List<CityAI> cities = new List<CityAI>();

        public string PlayerName { get { return PlayerNameStatic; } }
        public static string PlayerNameStatic { get { return "Johnny Bravo"; } }

        public Bicepsz()
        {
            startPoint = null;
        }

        public void RefreshWorldInfo(int turn, WorldInfo world)
        {
            if (this.startPoint == null)
                this.startPoint = world.GetMyCities().First().GetPosition();

            this.hiveMind = HiveMind.Generate(world, this.startPoint);
            this.units = world.GetMyUnits().Select(w => new UnitAI(w, this.hiveMind)).OrderBy(w => w.UnitInfo.GetPosition().DistanceTo(this.startPoint)).ToList();
            this.cities = world.GetMyCities().Select(w => new CityAI(w, this.hiveMind)).OrderBy(w => w.CityInfo.GetPosition().DistanceTo(this.startPoint)).ToList();
        }

        private Point attackingTarget;
        private UnitAI attackingUnit;
        public MovementData OnMovement()
        {
            if (this.attackingUnit != null)
            {
                List<UnitInfo> unitsOnTarget = this.hiveMind.World.Units.Where(w => w.PositionX == this.attackingTarget.X && w.PositionY == this.attackingTarget.Y).OrderByDescending(w => UnitStats.DefensePower[w.UnitTypeName]).ToList();
                if (unitsOnTarget.Any())
                {
                    UnitInfo target = unitsOnTarget.First();
                    unitsOnTarget.Remove(target);
                    this.hiveMind.World.Units = this.hiveMind.World.Units.Where(w => w.UnitID != target.UnitID).ToArray();
                }

                if (unitsOnTarget.Count == 0)
                {
                    this.attackingUnit.UnitInfo.PositionX = attackingTarget.X;
                    this.attackingUnit.UnitInfo.PositionY = attackingTarget.Y;
                    CityInfo attackedCity = this.hiveMind.World.Cities.FirstOrDefault(w => w.PositionX == attackingTarget.X && w.PositionY == attackingTarget.Y);
                    if (attackedCity != null)
                        attackedCity.Owner = PlayerName;
                }
                this.hiveMind.Regenerate();

                this.attackingUnit = null;
                this.attackingTarget = null;
            }


            foreach (UnitAI unit in this.units)
            {
                Point attackTarget = unit.OnAttack();
                if (attackTarget != null)
                {
                    this.attackingTarget = attackTarget;
                    this.attackingUnit = unit;

                    MovementData m = new MovementData();
                    m.FromX = unit.UnitInfo.PositionX;
                    m.FromY = unit.UnitInfo.PositionY;
                    m.ToX = attackTarget.X;
                    m.ToY = attackTarget.Y;
                    m.UnitID = unit.UnitInfo.UnitID;

                    unit.UnitInfo.MovementPoints--;
                    return m;
                }


                Point moveTarget = unit.OnMove();
                if (moveTarget != null)
                {
                    MovementData m = unit.UnitInfo.MoveTowards(this.hiveMind.World, moveTarget);
                    if (m != null)
                    {
                        this.hiveMind.Regenerate();
                        return m;
                    }
                }

                //Point attackTarget = unit.OnAttack();
                //if (attackTarget != null)
                //{
                //    this.attackingTarget = attackTarget;
                //    this.attackingUnit = unit;

                //    MovementData m = new MovementData();
                //    m.FromX = unit.UnitInfo.PositionX;
                //    m.FromY = unit.UnitInfo.PositionY;
                //    m.ToX = attackTarget.X;
                //    m.ToY = attackTarget.Y;
                //    m.UnitID = unit.UnitInfo.UnitID;
                //    return m;
                //}
            }
            return null;
        }

        public ResearchData OnResearch()
        {
            string whatToResearch = this.hiveMind.OnResearch();
            if (whatToResearch == null)
                return null;
            {
                ResearchData data = this.hiveMind.MyPlayer.Upgrade(whatToResearch);
                this.hiveMind.Regenerate();
                return data;
            }
        }

        public BuildingData OnBuilding()
        {
            foreach (UnitAI unit in this.units)
            {
                if (unit.OnBuilding())
                {
                    this.hiveMind.MyPlayer.Money -= 140;
                    CityInfo cityInfo = new CityInfo();
                    cityInfo.PositionX = unit.UnitInfo.PositionX;
                    cityInfo.PositionY = unit.UnitInfo.PositionY;
                    cityInfo.Owner = PlayerName;
                    this.hiveMind.World.Cities = this.hiveMind.World.Cities.Concat(new CityInfo[] { cityInfo }).ToArray();
                    this.cities.Add(new CityAI(cityInfo, this.hiveMind));
                    this.hiveMind.Regenerate();
                    return new BuildingData() { PositionX = cityInfo.PositionX, PositionY = cityInfo.PositionY };
                }
            }
            return null;
        }

        public TrainingData OnTraining()
        {
            this.hiveMind.Regenerate();
            foreach (CityAI city in this.cities)
            {
                CityAI.TrainCommand trainCommand = city.OnTrain();
                if (trainCommand == null)
                    continue;

                UnitInfo unitInfo = new UnitInfo();
                unitInfo.MovementPoints = 0;
                unitInfo.Owner = PlayerName;
                unitInfo.PositionX = city.CityInfo.PositionX;
                unitInfo.PositionY = city.CityInfo.PositionY;
                unitInfo.UnitID = null;
                unitInfo.UnitTypeName = trainCommand.Unit;

                TrainingData data = this.hiveMind.World.Buy(trainCommand.Unit, new Point(city.CityInfo.PositionX, city.CityInfo.PositionY));
                this.hiveMind.World.Units = this.hiveMind.World.Units.Concat(new UnitInfo[] { unitInfo }).ToArray();
                this.hiveMind.Regenerate();
                return data;
            }
            return null;
        }







        public void ActionResult(bool succeeded)
        {
            if (!succeeded)
            {
                if (this.attackingUnit != null)
                {
                    this.units.Remove(this.attackingUnit);
                    this.hiveMind.Regenerate();
                    this.attackingUnit = null;
                    this.attackingTarget = null;
                }
            }
        }

        public void CityLost(int positionX, int positionY)
        {
        }
        public void UnitLost(string unitID)
        {
            if (this.units.Any(w => w.UnitInfo.UnitID == unitID))
            {
                this.units.Remove(this.attackingUnit);
                this.hiveMind.World.Units = this.hiveMind.World.Units.Where(w => w.UnitID != unitID).ToArray();
                this.hiveMind.Regenerate();
            }
            if (this.attackingUnit != null)
            {
                this.attackingUnit = null;
                this.attackingTarget = null;
            }
        }

        public void EnemyDestroyed(string playerName)
        {
        }

        public void GameOver(bool winner, string message)
        {
        }

        public void ActionResult(WorldInfo world)
        {
            throw new NotImplementedException();
        }

        public string PlayerRace
        {
            get { throw new NotImplementedException(); }
        }
    }
}
