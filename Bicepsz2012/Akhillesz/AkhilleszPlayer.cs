using CivSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akhillesz
{
    public class AkhilleszPlayer : IPlayer
    {
        private static readonly String PLAYER_NAME = "Cockblock 3000";

        private Governor _arnold = new Governor(PLAYER_NAME);

        public void RefreshWorldInfo(int turn, WorldInfo world)
        {
            _arnold.WorldChanged(world);
        }

        public BuildingData OnBuilding()
        {
            return _arnold.DecideBuild();
        }

        public ResearchData OnResearch()
        {
            return _arnold.DecideResearch();
        }

        public TrainingData OnTraining()
        {
            return _arnold.DecideTrain();
        }

        public MovementData OnMovement()
        {
            return _arnold.DecideMove();
        }        

        public void ActionResult(WorldInfo world)
        {
            _arnold.WorldChanged(world);
        }
       
        // whatev
        public void CityLost(int positionX, int positionY)
        {
        }

        public void UnitLost(string unitID)
        {
        }

        public string PlayerName
        {
            get { return PLAYER_NAME; }
        }

        public string PlayerRace
        {
            get { return "civ2"; }
        }

        public void EnemyDestroyed(string playerName)
        {           
        }

        public void GameOver(bool winner, string message)
        {            
        }

    }
}
