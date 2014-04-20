using CivSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tricepsz.Actors;
using Tricepsz.Helpers;
using Tricepsz.Knowledge;
using Tricepsz.Strategies;

namespace Tricepsz
{
    public class TricepszPlayer : IPlayer
    {
        Actor ai;
        Debugger d;
        public TricepszPlayer()
        {
            ai = new Actor("Johnny Bravo II.", "civ1", new RushStrategy());
            d = new Debugger();
            d.Show();
        }

        public string PlayerName
        {
            get { return ai.Name; }
        }

        public string PlayerRace
        {
            get { return ai.Civ; }
        }

        public void RefreshWorldInfo(int turn, WorldInfo world)
        {
            // round started, save world info and do preliminary calculations
            ai.Map = new Map(world);
            ai.Round = turn;
            ai.UpdateObjectives();
        }

        public void ActionResult(WorldInfo world)
        {
            // we receive the effects of our action, might be useful to refresh world state after an attack or step
            // ai.Map = new Map(world);
            ai.MinorUpdateObjectives();
        }

        #region Events that require response (IN ORDER)

        public MovementData OnMovement()
        {
            return ai.PopNextMovementOrder();
        }

        public ResearchData OnResearch()
        {
            return ai.PopNextResearchOrder();
        }

        public BuildingData OnBuilding()
        {
            return ai.PopNextBuildingOrder();
        }

        public TrainingData OnTraining()
        {
            return ai.PopNextTrainingOrder();
        }

        #endregion

        #region Signals

        public void CityLost(int positionX, int positionY)
        {
            // city lost, might be useful to sign another city can be built
        }

        public void UnitLost(string unitID)
        {
            // unit lost, migt be usefult to sign that the unit cap can be filled once again
        }
        #endregion

        #region Useless
        public void EnemyDestroyed(string playerName)
        {
            // this is just a vestigial code of the 4 player mode
        }

        public void GameOver(bool winner, string message)
        {
            // whatever, we can't do anything after the game is over
        }
        #endregion
    }
}
