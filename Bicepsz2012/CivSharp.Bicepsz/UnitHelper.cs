using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CivSharp.Common;

namespace CivSharp.Bicepsz
{
    public static class UnitHelper
    {
        public static string GetDependency(this UnitInfo unit) { return Dependencies.UnitDependency[unit.UnitTypeName]; }
        public static int AttackPower(this UnitInfo unit) { return UnitStats.AttackPower[unit.UnitTypeName]; }
        public static int DefensePower(this UnitInfo unit) { return UnitStats.DefensePower[unit.UnitTypeName]; }
        public static int TotalMoves(this UnitInfo unit) { return UnitStats.TotalMoves[unit.UnitTypeName]; }
        public static int Cost(this UnitInfo unit) { return Costs.UnitCost[unit.UnitTypeName]; }
        public static Point GetPosition(this UnitInfo unit) { return new Point(unit.PositionX, unit.PositionY); }

        public static IEnumerable<Point> GetCellsInRange(this UnitInfo unit)
        {
            for (int i = unit.PositionX - unit.MovementPoints; i <= unit.PositionX + unit.MovementPoints; i++)
            {
                if (i < 0 || i >= 15)
                    continue;

                for (int j = unit.PositionY - unit.MovementPoints; j <= unit.PositionY + unit.MovementPoints; j++)
                {
                    if (j < 0 || j >= 15)
                        continue;

                    yield return new Point(i, j);
                }
            }
        }

        public static MovementData MoveTowards(this UnitInfo unit, WorldInfo world, Point target)
        {
            if (unit.MovementPoints <= 0)
                return null;
            if (unit.PositionX == target.X && unit.PositionY == target.Y)
                return null;

            MovementData movement = new MovementData() { FromX = unit.PositionX, FromY = unit.PositionY, UnitID = unit.UnitID };
            Point moveToPoint = world.GetNeighbouringCells(unit.GetPosition()).OrderBy(w => w.DistanceTo(target)).First();
            if (world.IsEnemyUnitOn(moveToPoint))
                return null;
            movement.ToX = moveToPoint.X;
            movement.ToY = moveToPoint.Y;
            unit.PositionX = moveToPoint.X;
            unit.PositionY = moveToPoint.Y;
            unit.MovementPoints--;
            return movement;
        }
    }
}
