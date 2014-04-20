using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tricepsz.Actors;
using CivSharp.Common;
using Tricepsz.Knowledge;

namespace Tricepsz.Strategies
{
    public class Strategy
    {
        public List<Objective> Objectives;
        public List<Order> Orders;
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

                Objectives.Add(new Objective()
                {
                    Name = "Build a scout",
                    Operation = (orderList) =>
                        {
                            if (Actor.Map.Units.Any(x => x.Type == Knowledge.UnitType.SCOUT)) return true;
                            else
                            {
                                var firstTown = Actor.Map.Cities.First(x=>x.Owner == Actor.Name);
                                var order = new TrainingData(){
                                    PositionX = firstTown.PositionX,
                                    PositionY = firstTown.PositionY,
                                    UnitTypeName = Unit.NameFromType(UnitType.SCOUT)
                                };
                                orderList.Add(new Order(order, OrderType.UNITBUY));
                                return false;
                            }

                        }
                });
                
            }
        }

        public void UpdateObjectives()
        {
            if (Orders == null) Orders = new List<Order>();
            List<Objective> finishedObjectives = new List<Objective>();
            foreach (var objective in Objectives)
            {
                if (objective.Execute())
                {
                    finishedObjectives.Add(objective);
                }
                Orders.AddRange(objective.Orders);
                objective.Orders.Clear();
            }

            Objectives.RemoveAll(x => finishedObjectives.Contains(x));
        }

        public void MinorUpdateObjectives()
        {
            
        }
    }
}
