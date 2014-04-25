using CivSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tricepsz.Knowledge;
using Tricepsz.Strategies;

namespace Tricepsz.Actors
{
    public class Actor
    {

        public Actor(string name, string civ, IStrategy strategy)
        {
            this.Name = name;
            this.Civ = civ;
            this.Strategy = strategy;
            Initialize();
        }

        public string Civ { get; set; }
        public string Name { get; set; }
        public IStrategy Strategy { get; set; }
        public Map Map { get; set; }
        public int Round { get; set; }
        public PlayerInfo Player { get; set; }

        public void Initialize()
        {
            Strategy.Actor = this;
            Strategy.SetGoal();
        }

        internal MovementData PopNextMovementOrder()
        {
            var nextOrder = Strategy.Orders.FirstOrDefault(x => x.CanExecute(OrderType.UNITMOVE));
            if (nextOrder == null) return null;
            var order = (MovementData)nextOrder.Execute<MovementData>();
            Strategy.Orders.Remove(nextOrder);
            return order;
        }

        internal ResearchData PopNextResearchOrder()
        {
            var nextOrder = Strategy.Orders.FirstOrDefault(x => x.CanExecute(OrderType.UPGRADE));
            if (nextOrder == null) return null;
            var order = (ResearchData)nextOrder.Execute<ResearchData>();
            Strategy.Orders.Remove(nextOrder);
            return order;
        }

        internal BuildingData PopNextBuildingOrder()
        {
            var nextOrder = Strategy.Orders.FirstOrDefault(x => x.CanExecute(OrderType.COLONYDEPLOY));
            if (nextOrder == null) return null;
            var order = (BuildingData)nextOrder.Execute<BuildingData>();
            Strategy.Orders.Remove(nextOrder);
            return order;
        }

        internal TrainingData PopNextTrainingOrder()
        {
            var nextOrder = Strategy.Orders.FirstOrDefault(x => x.CanExecute(OrderType.UNITBUY));
            if (nextOrder == null) return null;
            var order = (TrainingData)nextOrder.Execute<TrainingData>();
            Strategy.Orders.Remove(nextOrder);
            return order;
        }

        internal void MinorUpdateObjectives()
        {
            var me = this.Map.Players.First(x => x.Name == this.Name);
            this.Player = me;
            Strategy.MinorUpdateObjectives();
        }

        internal void UpdateObjectives()
        {
            var me = this.Map.Players.First(x => x.Name == this.Name);
            this.Player = me;
            Strategy.UpdateObjectives();
        }
    }
}
