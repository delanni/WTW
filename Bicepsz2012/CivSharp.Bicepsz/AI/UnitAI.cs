using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CivSharp.Common;

namespace CivSharp.Bicepsz
{
    public class UnitAI
    {
        private UnitInfo unitInfo;
        private HiveMind hiveMind;
        private bool build;

        public UnitInfo UnitInfo { get { return this.unitInfo; } }

        public UnitAI(UnitInfo unitInfo, HiveMind hiveMind)
        {
            this.unitInfo = unitInfo;
            this.hiveMind = hiveMind;
        }

        public Point OnMove()
        {
            if (this.unitInfo.MovementPoints <= 0) // ha nincs tobb movement lehetoseg vagy mar epitesre keszulok
                return null;

            Point urgentCell = this.unitInfo.GetCellsInRange().Where(w => this.hiveMind.DomainBorder[w.X, w.Y] &&
                !this.hiveMind.IsSafety[w.X, w.Y] &&
                !this.hiveMind.World.IsCityOn(w) &&
                !this.hiveMind.World.IsUnitOn(w, Bicepsz.PlayerNameStatic)).FirstOrDefault();
            if (urgentCell != null) // ha van olyan elerheto border pont ahol nincs varos es tamadhato es meg nincs ott egyseg
            {
                // odalepek es keszulok az epitkezesre
                this.build = true;
                return urgentCell;
            }

            if (!this.hiveMind.World.IsCityOn(this.unitInfo.GetPosition()) && (this.unitInfo.PositionX + this.unitInfo.PositionY) % 2 == 0)
            {
                // odalepek es keszulok az epitkezesre
                this.build = true;
                return this.unitInfo.GetPosition();
            }

            Point expansionCell = this.unitInfo.GetCellsInRange().Where(w => (w.X + w.Y) % 2 == 0 &&
                !this.hiveMind.World.IsCityOn(w) &&
                !this.hiveMind.World.IsUnitOn(w, Bicepsz.PlayerNameStatic))
                .OrderBy(w => w.DistanceTo(this.hiveMind.SpawnPoint))
                .FirstOrDefault();
            if (expansionCell != null) // ha van az paros osszegu elerheto mezok kozul hely ahova varost telepithetek, es meg nincs ott egyseg
            {
                // odalepek es keszulok az epitkezesre
                this.build = true;
                return expansionCell;
            }

            Point innerCell = this.unitInfo.GetCellsInRange().Where(w => this.hiveMind.IsSafety[w.X, w.Y] &&
                !this.hiveMind.World.IsCityOn(w) &&
                !this.hiveMind.World.IsUnitOn(w, Bicepsz.PlayerNameStatic)).FirstOrDefault();
            if (innerCell != null) // az elerheto mezok kozul van olyan safe ures mezo ahol varos sincs es jatekos se (belso terulet)
            {
                // odalepek es keszulok az epitkezesre
                this.build = true;
                return innerCell;
            }

            if (this.hiveMind.DomainBorder[this.unitInfo.PositionX, this.unitInfo.PositionY])
                return null;

            //Point leastDefendedPoint = this.hiveMind.DomainBorder.GetPoints().OrderBy(w => hiveMind.DefensePowers[w.X, w.Y]).FirstOrDefault();

            //return leastDefendedPoint;
            Point mostAttackedPoint = this.hiveMind.DomainBorder.GetPoints().OrderByDescending(w => hiveMind.PossibleAttackPowers[w.X, w.Y] / w.DistanceTo(this.unitInfo.GetPosition())).FirstOrDefault();

            return mostAttackedPoint;

            //emez már first or default
            //return null;




            //if (true) // ha foloslegesse vallok a vedelemben
            //{
            //}
            //if (true) // nincs a szomedban dolog
            //{
            //    // fogom a border pontjait
            //    // sorrendezem az ott tartozkodo ijaszok alapjan
            //    // elindulok az elso ilyen fele
            //}
            //else
            //    return null;
        }

        public Point OnAttack()
        {
            if (this.unitInfo.MovementPoints <= 0)
                return null;

            Point[] a = hiveMind.World.GetNeighbouringCells(unitInfo.GetPosition()).Where(w => hiveMind.World.IsEnemyCityOn(new Point(w.X, w.Y)) && !hiveMind.World.IsEnemyUnitOn(new Point(w.X, w.Y))).ToArray();
            if (a.Any())
                return a.FirstOrDefault();

            if (this.hiveMind.DefensePowers[this.unitInfo.PositionX, this.unitInfo.PositionY] > this.hiveMind.PossibleAttackPowers[this.unitInfo.PositionX, this.unitInfo.PositionY] * 3)
            {
                Point target = this.hiveMind.World.GetNeighbouringCells(this.unitInfo.GetPosition()).FirstOrDefault(w => this.hiveMind.World.IsEnemyCityOn(w));
                if (target != null)
                    return target;
            }
            return null;
        }

        public bool OnBuilding()
        {
            // ha keszultem az epitkezesre, akkor epitkezek
            return this.build && this.hiveMind.MyPlayer.Money >= 140 && !this.hiveMind.World.IsCityOn(this.unitInfo.PositionX, this.unitInfo.PositionY);
        }
    }
}
