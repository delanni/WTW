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
    public class RushStrategy : Tricepsz.Strategies.IStrategy
    {
        public List<Objective> Objectives { get; set; }
        public List<Order> Orders {get; set; }
        public Actor Actor { get; set; }

        private Objective buildInifiteScouts;

        public RushStrategy()
        {
            buildInifiteScouts = new Objective()
                {
                    Name = "Build infinite scouts",
                    Operation = (orderList) =>
                        {
                            if (Actor.Map.Units.Count(x => x.Type == Knowledge.UnitType.SCOUT)>200) return true;
                            else
                            {
                                var firstTown = Actor.Map.Cities.First(x => x.Owner == Actor.Name);

                                var order = new TrainingData()
                                {
                                    PositionX = firstTown.PositionX,
                                    PositionY = firstTown.PositionY,
                                    UnitTypeName = Unit.NameFromType(UnitType.SCOUT)
                                };
                                if (Actor.Player.Money > Unit.SCOUT.COST)
                                orderList.Add(new Order(order, OrderType.UNITBUY));
                                return false;
                            }

                        }
                };
        }

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

                Objectives.Add(buildInifiteScouts);
                
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
