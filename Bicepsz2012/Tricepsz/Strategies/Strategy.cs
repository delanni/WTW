using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tricepsz.Actors;

namespace Tricepsz.Strategies
{
    public class Strategy
    {
        public List<Objective> Objectives;
        public Actor Actor;

        public void SetGoal()
        {
            if (Objectives == null)
            {
                Objectives.Add(new Objective()
                {
                    Name = "Win",
                    Operation = (orderList) =>
                    {
                        return Actor.Map.Players.All(x => x.Name == Actor.Name);
                    }
                });
            }
        }

        public void UpdateObjectives()
        {
            
        }

        public void MinorUpdateObjectives()
        {
            
        }
    }
}
