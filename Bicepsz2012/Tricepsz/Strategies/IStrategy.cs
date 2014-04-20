using System;
using System.Collections.Generic;
using Tricepsz.Actors;
namespace Tricepsz.Strategies
{
    interface IStrategy
    {
        Actor Actor { get; set; }
        void MinorUpdateObjectives();
        List<Objective> Objectives { get; set; }
        List<Order> Orders { get; set; }
        void SetGoal();
        void UpdateObjectives();
    }
}
