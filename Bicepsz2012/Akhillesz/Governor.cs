using CivSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Akhillesz
{
    class Governor
    {        
        private WorldInfo _currentWorld;

        private PlayerInfo _me;

        private CommandBag _commandBag = new CommandBag();

        private string _playerName;

        public Governor(string playerName)
        {            
            _playerName = playerName;
        }

        internal void WorldChanged(WorldInfo world)
        {
            this._currentWorld = world;
            _commandBag.Clear();            
            _me = world.Players.First( m => m.Name.Equals(_playerName) );
            DecideWhoDoesWhat();
        }

        internal void DecideWhoDoesWhat()
        {
            List<UnitInfo> myUnits = _currentWorld.Units.Where( m => m.Owner.Equals(_playerName) ).ToList();

            foreach (UnitInfo unit in myUnits)
            {
                if (unit.MovementPoints > 0)
                {
                    ITask bestTask = TaskFactory.BestTaskForUnit(_me, _currentWorld, unit);
                    if (bestTask != null)
                    {
                        bestTask.Execute(_currentWorld, _commandBag);
                    }
                }                
            }

            if (myUnits.Count < 5)
            {
                CityInfo firstCity = _currentWorld.Cities.First(m => m.Owner.Equals(_playerName));

                UnitProperties fodder = Units.Types.First(u => u.Name.Equals("felderítő"));

                if (_me.Money > fodder.Cost)
                {
                    _commandBag.AddTrain(new TrainingData { PositionX = firstCity.PositionX, PositionY = firstCity.PositionY, UnitTypeName = fodder.Name } );
                }                
            }            
        }

        internal ResearchData DecideResearch()
        {
            return _commandBag.PopResearch();
        }

        internal TrainingData DecideTrain()
        {           
            return _commandBag.PopTraining();
        }

        internal MovementData DecideMove()
        {
            return _commandBag.PopMovement();
        }

        internal BuildingData DecideBuild()
        {
            return _commandBag.PopBuilding();
        }
    }

    class CommandBag : ICommandQueueTaskInterface
    {
        private Queue<ResearchData> _research = new Queue<ResearchData>();

        private Queue<TrainingData> _training = new Queue<TrainingData>();

        private Queue<MovementData> _movement = new Queue<MovementData>();

        private Queue<BuildingData> _building = new Queue<BuildingData>();
       
        public void Clear()
        {
            _research.Clear();
            _training.Clear();
            _movement.Clear();
            _building.Clear();
        }

        public void AddMovement(MovementData movementData)
        {
            _movement.Enqueue(movementData);
        }

        public void AddResearch(ResearchData researchData)
        {
            _research.Enqueue(researchData);
        }

        public void AddBuild(BuildingData buildingData)
        {
            _building.Enqueue(buildingData);
        }

        public void AddTrain(TrainingData trainingData)
        {
            _training.Enqueue(trainingData);
        }

        public MovementData PopMovement()
        {            
            return _movement.Count > 0 ? _movement.Dequeue() : null;
        }

        public ResearchData PopResearch()
        {
            return _research.Count > 0 ? _research.Dequeue() : null;
        }

        public TrainingData PopTraining()
        {            
            return _training.Count > 0 ? _training.Dequeue() : null;
        }

        public BuildingData PopBuilding()
        {
            return _building.Count > 0 ? _building.Dequeue() : null;
        }
    }
}
