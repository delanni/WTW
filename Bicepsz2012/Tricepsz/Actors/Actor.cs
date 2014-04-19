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

        public Actor(string name, string civ, Strategy strategy)
        {
            this.Name = name;
            this.Civ = civ;
            this.Strategy = strategy;
            Initialize();
        }

        public string Civ { get; set; }
        public string Name { get; set; }
        public Strategy Strategy { get; set; }
        public Map Map { get; set; }

        public void Initialize()
        {
            Strategy.Actor = this;
            Strategy.SetGoal();
        }

        internal MovementData PopNextMovementOrder()
        {
            return null;
        }

        internal ResearchData PopNextResearchOrder()
        {
            return null;
        }

        internal BuildingData PopNextBuildingOrder()
        {
            return null;
        }

        internal TrainingData PopNextTrainingOrder()
        {
            return null;
        }

        internal void MinorUpdateObjectives()
        {
            Strategy.MinorUpdateObjectives();
        }

        internal void UpdateObjectives()
        {
            Strategy.UpdateObjectives();
        }

        public int Round { get; set; }
    }
}
