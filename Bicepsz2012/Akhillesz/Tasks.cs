﻿using CivSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Akhillesz
{
    interface ICommandQueueTaskInterface
    {
        void AddMovement(MovementData movementData);
        void AddResearch(ResearchData researchData);
        void AddBuild(BuildingData buildingData);
        void AddTrain(TrainingData trainingData);
    }

    abstract class ITask : IComparable<ITask>
    {        
        public abstract void Execute(WorldInfo currentWorld, ICommandQueueTaskInterface intf);
        public abstract double GetScore();

        public int CompareTo(ITask other) {            
            return other.GetScore().CompareTo(GetScore());
        }
    }

    class AttackCity : ITask
    {
        private static readonly int[][] EIGHT_WAY = new int[][] {
            new int[] { -1, -1 }, 
            new int[] { 0, -1 }, 
            new int[] { 1, -1 }, 
            new int[] { 1, 0 } ,
            new int[] { 1, 1 } ,
            new int[] { 0, 1 } ,
            new int[] { -1, 1 } ,
            new int[] { -1, 0 } ,
        };

        private UnitInfo _unit;
        private CityInfo _whichCity;
        private UnitProperties _unitProps;

        public AttackCity(UnitInfo unit, CityInfo whichCity)
        {
            _unit = unit;
            _unitProps = Units.Types.Find(m => m.Name == _unit.UnitTypeName);
            _whichCity = whichCity;
        }

        public override void Execute(WorldInfo currentWorld, ICommandQueueTaskInterface intf)
        {
            if ((_whichCity.PositionX != _unit.PositionX || _whichCity.PositionY != _unit.PositionY))
            {
                List<Point> candidates = new List<Point>();

                for (int i = 0; i < EIGHT_WAY.Length; i++)
                {
                    int cX = _unit.PositionX + EIGHT_WAY[i][0];
                    int cY = _unit.PositionY + EIGHT_WAY[i][1];

                    if ( cX >= 0 && cY >= 0 && cX < 20 && cY < 20 && !currentWorld.Units.Any(m => m.Owner != _unit.Owner && m.PositionX == cX && m.PositionY == cY))
                    {
                        candidates.Add(new Point { X = cX, Y = cY });
                    }                    
                }

                Point to;                

                if (candidates.Count == 0)
                {
                    to = new Point { 
                        X = _unit.PositionX + Math.Sign(_whichCity.PositionX - _unit.PositionX),
                        Y = _unit.PositionY + Math.Sign(_whichCity.PositionY - _unit.PositionY)
                    };                    
                } else {
                    to = candidates.First();

                    foreach ( Point p in candidates ) {
                        if (p.DistanceFrom(_whichCity.PositionX, _whichCity.PositionY) < to.DistanceFrom(_whichCity.PositionX, _whichCity.PositionY))
                        {
                            to = p;
                        }
                    }

                }

                intf.AddMovement(new MovementData
                {
                    UnitID = _unit.UnitID,

                    FromX = _unit.PositionX,
                    FromY = _unit.PositionY,

                    ToX = to.X,
                    ToY = to.Y
                });
            }
        }

        public override double GetScore()
        {
            return 4 + _unitProps.Attack + _unitProps.Defense + _unitProps.Health;
        }
    }

    class DefendCity : ITask
    {
        private UnitInfo _unit;
        private CityInfo _whichCity;
        private UnitProperties _unitProps;

        public DefendCity(UnitInfo unit, CityInfo whichCity)
        {
            _unit = unit;
            _unitProps = Units.Types.Find(m => m.Name == _unit.UnitTypeName);
            _whichCity = whichCity;
        }

        public override void Execute(WorldInfo currentWorld, ICommandQueueTaskInterface intf)
        {
            if (_whichCity.PositionX != _unit.PositionX || _whichCity.PositionY != _unit.PositionY)
            {
                intf.AddMovement(new MovementData
                {
                    UnitID = _unit.UnitID,
                    FromX = _unit.PositionX,
                    FromY = _unit.PositionY,

                    ToX = _unit.PositionX + Math.Sign( _whichCity.PositionX - _unit.PositionX ),
                    ToY = _unit.PositionY + Math.Sign( _whichCity.PositionY - _unit.PositionY )
                });
            }
        }

        public override double GetScore()
        {
            return 3 + _unitProps.Attack + _unitProps.Defense + _unitProps.Health;
        }
    }

    class AttackUnit : ITask
    {
        private UnitInfo _unit;
        private UnitInfo _targetUnit;
        private UnitProperties _unitProps;

        public AttackUnit(UnitInfo unit, UnitInfo targetUnit)
        {
            _unit = unit;
            _unitProps = Units.Types.Find(m => m.Name == _unit.UnitTypeName);
            _targetUnit = targetUnit;
        }

        public override void Execute(WorldInfo currentWorld, ICommandQueueTaskInterface intf)
        {
            if (_targetUnit.PositionX != _unit.PositionX || _targetUnit.PositionY != _unit.PositionY)
            {
                intf.AddMovement(new MovementData
                {
                    UnitID = _unit.UnitID,
                    FromX = _unit.PositionX,
                    FromY = _unit.PositionY,

                    ToX = _unit.PositionX + Math.Sign(_targetUnit.PositionX - _unit.PositionX),
                    ToY = _unit.PositionY + Math.Sign(_targetUnit.PositionY - _unit.PositionY)
                });
            }
        }

        public override double GetScore()
        {
            return 2 + _unitProps.Attack + _unitProps.Defense + _unitProps.Health;
        }
    }

    class BuildingTask : ITask
    {
        private UnitInfo _unit;
        private int _x;
        private int _y;

        public BuildingTask(UnitInfo unit, int x, int y)
        {
            _unit = unit;
            _x = x;
            _y = y;
        }

        public override void Execute(WorldInfo currentWorld, ICommandQueueTaskInterface intf)
        {
            if (_x != _unit.PositionX || _y != _unit.PositionY)
            {
                intf.AddMovement(new MovementData
                {
                    UnitID = _unit.UnitID,
                    FromX = _unit.PositionX,
                    FromY = _unit.PositionY,

                    ToX = _unit.PositionX + Math.Sign(_x - _unit.PositionX),
                    ToY = _unit.PositionY + Math.Sign(_y - _unit.PositionY)
                });
            }
            else
            {
                if (!currentWorld.Cities.Any(m => m.PositionX == _x && m.PositionY == _y))
                {
                    intf.AddBuild(new BuildingData { PositionX = _x, PositionY = _y });
                }
            }
        }

        public override double GetScore()
        {
            return 1;
        }
    }

    static class TaskFactory
    {
        public static ITask BestTaskForUnit(PlayerInfo player, WorldInfo world, UnitInfo unit)
        {
            // nincs figyelembe véve hogy egy körben mennyit léphetünk

            if (unit.UnitTypeName.Equals("őrző")) return null;

            List<ITask> feasibleTasks = new List<ITask>();            

            // lehet feltételes is, ha nincs egy bizonyos távolságon belül az ellenfél akkor nem törődik vele
            foreach (CityInfo city in world.Cities.Where(m => m.Owner == player.Name)) {
                feasibleTasks.Add(new DefendCity(unit, city));
            }

            foreach (CityInfo city in world.Cities.Where(m => m.Owner != player.Name))
            {
                feasibleTasks.Add(new AttackCity(unit, city));
            }

            foreach (UnitInfo enemyUnit in world.Units.Where(m => m.Owner != player.Name)) {
                feasibleTasks.Add(new AttackUnit(unit, enemyUnit));
            }

            for (int x = 0; x < 20; x++ )
            {
                for (int y = 0; y < 20; y++)
                {
                    if (!world.Cities.Any(m => m.PositionX == x && m.PositionY == y && m.Owner != player.Name) ||
                        !world.Units.Any(m => m.PositionX == x && m.PositionY == y && m.Owner != player.Name))
                    {
                        feasibleTasks.Add(new BuildingTask(unit, x, y));
                    }                    
                }
            }

            feasibleTasks.Sort();

            return feasibleTasks.FirstOrDefault();
        }
    }

    static class UnitInfoExtensions
    {
        public static double DistanceTo(this UnitInfo me, UnitInfo other)
        {
            return Math.Sqrt( Math.Abs(me.PositionX - other.PositionX) + Math.Abs(me.PositionY - other.PositionY) );
        }

        public static double DistanceTo(this UnitInfo me, CityInfo city)
        {
            return Math.Sqrt(Math.Abs(me.PositionX - city.PositionX) + Math.Abs(me.PositionY - city.PositionY));
        }
    }
}
