using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CivSharp.Common;

namespace CivSharp.Bicepsz
{
    public class HiveMind
    {
        public WorldInfo World { get; private set; }
        public PlayerInfo MyPlayer { get; private set; }
        public bool[,] IsSafety { get; private set; }
        public bool[,] DomainBorder { get; private set; }
        public int[,] PossibleAttackPowers { get; private set; }
        public double[,] DefensePowers { get; private set; }
        public Point SpawnPoint { get; private set; }

        private HiveMind() { }

        public static HiveMind Generate(WorldInfo world, Point spawnPoint)
        {
            HiveMind hiveMind = new HiveMind();
            hiveMind.SpawnPoint = spawnPoint;
            hiveMind.World = world;
            hiveMind.MyPlayer = world.GetMyPlayer();
            hiveMind.IsSafety = world.GetCellSafeties();
            hiveMind.DomainBorder = world.GetDomainBorder();
            hiveMind.PossibleAttackPowers = world.GetPossibleAttackSum();
            hiveMind.DefensePowers = world.GetDefenseSum();
            return hiveMind;
        }

        public void Regenerate()
        {
            IsSafety = World.GetCellSafeties();
            DomainBorder = World.GetDomainBorder();
            PossibleAttackPowers = World.GetPossibleAttackSum();
            DefensePowers = World.GetDefenseSum();
        }

        public string OnResearch()
        {
            PlayerInfo myPlayer = World.GetMyPlayer();
            if (myPlayer.CanResearch(Research.iras) && World.GetIncome(MyPlayer.Name, true, false) > World.GetIncome(1))
            {
                return Research.iras;
            }
            else if (myPlayer.CanResearch(Research.birosag) && World.GetIncome(MyPlayer.Name, true, true) > World.GetIncome(1))
            {
                return Research.birosag;
            }
            else if (myPlayer.Researched.Contains(Research.birosag))
            {
                if (myPlayer.CanResearch(Research.famegmunkalas))
                {
                    return Research.famegmunkalas;
                }
                if (myPlayer.CanResearch(Research.ijaszat))
                    return Research.ijaszat;
            }

            if (myPlayer.CanResearch(Research.colopkerites) && World.GetIncome() > 600)
            {
                return Research.colopkerites;
            }
            if (myPlayer.CanResearch(Research.kofal) && World.GetIncome() > 800)
            {
                return Research.kofal;
            }
            return null;
        }
    }
}
