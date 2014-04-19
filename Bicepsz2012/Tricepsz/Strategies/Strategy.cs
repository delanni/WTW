using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
                Objectives = new List<Objective>();
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
            List<Objective> finishedObjectives = new List<Objective>();
            foreach (var objective in Objectives)
            {
                if (objective.Execute())
                {
                    finishedObjectives.Add(objective);
                }
            }

            Objectives.RemoveAll(x => finishedObjectives.Contains(x));
        }

        public void MinorUpdateObjectives()
        {
            
        }
    }
}
