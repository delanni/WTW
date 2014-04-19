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
            throw new NotImplementedException();
        }

        internal ResearchData PopNextResearchOrder()
        {
            throw new NotImplementedException();
        }

        internal BuildingData PopNextBuildingOrder()
        {
            throw new NotImplementedException();
        }

        internal TrainingData PopNextTrainingOrder()
        {
            throw new NotImplementedException();
        }

        internal void MinorUpdateObjectives()
        {
            Strategy.MinorUpdateObjectives();
        }

        internal void UpdateObjectives()
        {
            Strategy.UpdateObjectives();
        }
    }
}
