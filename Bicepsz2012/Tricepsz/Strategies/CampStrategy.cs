using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tricepsz.Actors;
using CivSharp.Common;
using Tricepsz.Knowledge;
using Tricepsz.Helpers;

namespace Tricepsz.Strategies
{
    public class CampStrategy : Tricepsz.Strategies.IStrategy
    {
        public List<Objective> Objectives { get; set; }
        public List<Order> Orders { get; set; }
        public Actor Actor { get; set; }

        private Objective buildInifiteArmy;
        private Objective foundInfiniteColonies;
        private Objective moveUnits;
        private Objective buildABarack;
        private Objective buildABlackSmith;
        private Objective buildATower;
        private Objective buildATown;

        public CampStrategy()
        {
            Research.Initialize();

            #region Build army
            buildInifiteArmy = new Objective()
                {
                    Name = "Build army",
                    Operation = (orderList) =>
                        {
                            if (Actor.Map.Units.Count(x => x.Type == Knowledge.UnitType.SCOUT) > 200) return true;
                            else
                            {
                                var myUnits = Actor.Map.Units.Where(x => x.Owner == Actor.Name);
                                var myTowns = Actor.Map.Cities.Where(x => x.Owner == Actor.Name);

                                var maxUnitCount = myTowns.Count() * 5;
                                if (myUnits.Count() >= maxUnitCount) return false;

                                var otherTowns = Actor.Map.Cities.Except(myTowns);
                                var distancePairs = myTowns.Select(x => new { Town = x, Distance = otherTowns.Min(y => Position.StepsTo(x, y)) });
                                var nearestTown = distancePairs.OrderBy(x => x.Distance).First().Town;

                                var topUnit = Unit.GetTopUnitFor(Actor.Player, true);
                                if (topUnit.Type == UnitType.GUARD && myUnits.Count(x => x.Type == UnitType.GUARD) >= 0.8 * maxUnitCount)
                                {
                                    topUnit = Unit.SCOUT;
                                }

                                var order = new TrainingData()
                                {
                                    PositionX = nearestTown.PositionX,
                                    PositionY = nearestTown.PositionY,
                                    UnitTypeName = topUnit.UnitTypeName
                                };

                                if (topUnit.CanBuild(Actor.Player) )
                                    orderList.Add(new Order(order, OrderType.UNITBUY));
                                return false;
                            }

                        }
                };

            #endregion

            #region BuildColonies
            foundInfiniteColonies = new Objective()
            {
                Name = "FoundColonies",
                Operation = (orderList) =>
                {
                    if (Actor.Map.Cities.Count(x => x.Owner == Actor.Name) >= 4) return true;
                    else
                    {
                        if (Actor.Player.Money < 300) return false;
                        var ourGuys = Actor.Map.Units.Where(x => x.Owner == Actor.Name);
                        var theirTowns = Actor.Map.Cities.Where(x => x.Owner != Actor.Name);
                        if (ourGuys.Count() == 0 || theirTowns.Count() == 0) return false;
                        var distancePairs = ourGuys.Select(x => new { Town = x, Distance = theirTowns.Min(y => Position.StepsTo(x, y)) });

                        var nearestGuy = distancePairs.OrderBy(x => x.Distance).First().Town;

                        if (Actor.Player.Money > 300)
                        {
                            orderList.Add(new Order(new CivSharp.Common.BuildingData()
                            {
                                PositionX = nearestGuy.Position.X,
                                PositionY = nearestGuy.Position.Y
                            }, OrderType.COLONYDEPLOY));
                        }
                        return false;
                    }
                }

            };

            #endregion

            #region Move Scouts

            moveUnits = new Objective()
            {
                Name = "Move",
                Operation = (orderList) =>
                {
                    foreach (var scout in Actor.Map.Units.Where(x => x.Owner == Actor.Name))
                    {
                        if (scout.MovementPoints <= 0) continue;
                        var enemyTowns = Actor.Map.Cities.Where(x => x.Owner != Actor.Name);
                        var ourTowns = Actor.Map.Cities.Except(enemyTowns);
                        var townsWithDistances = enemyTowns.Select(x => new { Town = x, Distance = Position.StepsTo(scout, x) });

                        var nearestTown = townsWithDistances.OrderBy(x => x.Distance).First().Town;
                        var outTownsWithDistances = ourTowns.Select(x => new { Town = x, Distance = Position.StepsTo(nearestTown, x) });
                        var ourNearestTown = outTownsWithDistances.OrderBy(x => x.Distance).First().Town;

                        var theStepNextToOurTown = Position.NextStep(ourNearestTown, nearestTown);

                        var nextStep = Position.NextStep(scout, theStepNextToOurTown);

                        orderList.Add(new Order(new CivSharp.Common.MovementData()
                        {
                            FromX = scout.Position.X,
                            FromY = scout.Position.Y,
                            UnitID = scout.UnitId,
                            ToX = nextStep.X,
                            ToY = nextStep.Y
                        }, OrderType.UNITMOVE));
                    }
                    return true;
                }
            };

            #endregion


            buildABarack = buildA(Research.BARRACKS);
            buildABlackSmith = buildA(Research.BLACKSMITH);
            buildATower = buildA(Research.TOWER);
            buildATown = buildA(Research.TOWN);
        }

        private Objective buildA(Research research)
        {
            var obj = new Objective()
            {
                Name = "Build a " + research.Name,
                Operation = (list) =>
                {
                    if (Actor.Player.Researched.Contains(research.Name)) return true;
                    else
                    {
                        if (Actor.Player.Money >= research.Cost)
                        {
                            list.Add(new Order(new ResearchData()
                            {
                                WhatToResearch = research.Name
                            }, OrderType.UPGRADE)); return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            };

            return obj;
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

                Objectives.Add(buildATower);
                //Objectives.Add(buildABarack);
                //Objectives.Add(buildABlackSmith);
                Objectives.Add(buildInifiteArmy);
                Objectives.Add(foundInfiniteColonies);
            }
        }

        public void UpdateObjectives()
        {
            if (Orders == null) Orders = new List<Order>();

            if (Orders.Count(x => x.Type == OrderType.UNITMOVE) == 0) Objectives.Add(moveUnits);

            var oldBuyOrders = Orders.Where(x => x.Type == OrderType.UNITBUY);
            Orders.RemoveAll(x=>oldBuyOrders.Contains(x));

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
            if (Orders == null) Orders = new List<Order>();

            if (Orders.Count(x => x.Type == OrderType.UNITMOVE) == 0) Objectives.Add(moveUnits);

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
    }
}
