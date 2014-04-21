using CivSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tricepsz.Knowledge
{
    class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public double DistanceTo(Position other)
        {
            int dx = other.X - X;
            int dy = other.Y - Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public int StepsTo(Position other)
        {
            int dx = other.X - X;
            int dy = other.Y - Y;
            return Math.Abs(dx) + Math.Abs(dy);
        }

        public static int StepsTo(CityInfo one, CityInfo other)
        {
            return Math.Abs(one.PositionX - other.PositionX) + Math.Abs(one.PositionY - other.PositionY);
        }

        public static int StepsTo(Unit one, Unit other)
        {
            return Math.Abs(one.Position.X - other.Position.X) + Math.Abs(one.Position.Y - other.Position.Y);
        }

        public static int StepsTo(Unit one, CityInfo other)
        {
            return Math.Abs(one.Position.X - other.PositionX) + Math.Abs(one.Position.Y - other.PositionY);
        }

        internal static Position NextStep(CityInfo oneTown, CityInfo nearestTown)
        {
            var nextStep = new Position(oneTown.PositionX, oneTown.PositionY);
            if (DateTime.Now.Millisecond % 2 == 0)
            {
                if (oneTown.PositionX != nearestTown.PositionX)
                {
                    nextStep.X += Math.Sign(nearestTown.PositionX - oneTown.PositionX);
                }
                else
                {
                    nextStep.Y += Math.Sign(nearestTown.PositionY - oneTown.PositionY);
                }
            }
            else
            {
                if (oneTown.PositionY != nearestTown.PositionY)
                {
                    nextStep.Y += Math.Sign(nearestTown.PositionY - oneTown.PositionY);
                }
                else
                {
                    nextStep.X += Math.Sign(nearestTown.PositionX - oneTown.PositionX);
                }
            }
            return nextStep;
        }

        internal static Position NextStep(Unit scout, CityInfo nearestTown)
        {
            var nextStep = new Position(scout.Position.X, scout.Position.Y);
            if (DateTime.Now.Millisecond % 2 == 0)
            {
                if (scout.Position.X != nearestTown.PositionX)
                {
                    nextStep.X += Math.Sign(nearestTown.PositionX - scout.Position.X);
                }
                else
                {
                    nextStep.Y += Math.Sign(nearestTown.PositionY - scout.Position.Y);
                }
            }
            else
            {
                if (scout.Position.Y != nearestTown.PositionY)
                {
                    nextStep.Y += Math.Sign(nearestTown.PositionY - scout.Position.Y);
                }
                else
                {
                    nextStep.X += Math.Sign(nearestTown.PositionX - scout.Position.X);
                }
            }
            return nextStep;
        }

        internal static Position NextStep(Unit scout, Position theStepNextToOurTown)
        {
            return NextStep(scout, new CityInfo() { PositionX = theStepNextToOurTown.X, PositionY = theStepNextToOurTown.Y });
        }
    }
}
