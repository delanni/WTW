using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CivSharp.Common;
using System.Diagnostics;

namespace CivSharp.Bicepsz
{
    public static class WorldInfoExtensions
    {
        #region CityInfo

        public static Point GetPosition(this CityInfo city)
        {
            return new Point(city.PositionX, city.PositionY);
        }
        public static IEnumerable<CityInfo> GetMyCities(this WorldInfo world)
        {
            return world.Cities.Where(w => w.Owner == Bicepsz.PlayerNameStatic);
        }
        public static IEnumerable<CityInfo> GetOthersCities(this WorldInfo world)
        {
            return world.Cities.Where(w => w.Owner != Bicepsz.PlayerNameStatic);
        }

        #endregion

        #region Player

        public static PlayerInfo GetMyPlayer(this WorldInfo world)
        {
            return world.Players.Single(w => w.Name == Bicepsz.PlayerNameStatic);
        }

        #endregion

        #region Units

        public static IEnumerable<UnitInfo> GetMyUnits(this WorldInfo world)
        {
            return world.Units.Where(w => w.Owner == Bicepsz.PlayerNameStatic);
        }
        public static IEnumerable<UnitInfo> GetOthersUnits(this WorldInfo world)
        {
            return world.Units.Where(w => w.Owner != Bicepsz.PlayerNameStatic);
        }

        #endregion

        #region Cells

        public static bool[,] GetCellSafeties(this WorldInfo world)
        {
            bool[,] safeties = new bool[15, 15];
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                    safeties[i, j] = true;
            }

            foreach (UnitInfo unitInfo in world.GetOthersUnits())
            {
                for (int i = unitInfo.PositionX - unitInfo.MovementPoints; i <= unitInfo.PositionX + unitInfo.MovementPoints; i++)
                {
                    if (i < 0 || i >= 15)
                        continue;
                    for (int j = unitInfo.PositionY - unitInfo.MovementPoints; j < unitInfo.PositionY + unitInfo.MovementPoints; j++)
                    {
                        if (j < 0 || j >= 15)
                            continue;

                        safeties[i, j] = false;
                    }
                }
            }

            return safeties;
        }
        public static IEnumerable<Point> GetSafePoints(this WorldInfo world)
        {
            bool[,] safeties = world.GetCellSafeties();
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (safeties[i, j])
                        yield return new Point(i, j);
                }
            }
        }
        public static bool[,] GetDomainBorder(this WorldInfo world)
        {
            bool[,] domainBorder = new bool[15, 15];
            CityInfo[] cities = world.GetMyCities().ToArray();
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (cities.IsBorderCell(i, j))
                        domainBorder[i, j] = true;
                    else
                    {
                        CityInfo city = cities.SingleOrDefault(w => w.PositionX == i && w.PositionY == j);
                        if (city != null)
                            domainBorder[i, j] = world.GetNeighbouringCells(city.GetPosition()).Any(w => world.IsEnemyCityOn(w));
                    }
                }

            }

            return domainBorder;
        }
        public static IEnumerable<Point> GetPoints(this bool[,] values)
        {
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    if (values[i, j])
                        yield return new Point(i, j);
                }
            }
        }
        public static bool IsBorderCell(this IEnumerable<CityInfo> cities, int x, int y)
        {
            if (cities.Any(w => w.PositionX == x && w.PositionY == y))
            {
                return cities.CountBlockingDiagonalNeighboursCount(x, y) < 4;
            }
            else
            {
                int count = cities.CountBlockingHorizontalAndVerticalNeighboursCount(x, y);
                return count == 2 || count == 3;
            }
        }
        public static int CountBlockingHorizontalAndVerticalNeighboursCount(this IEnumerable<CityInfo> cities, int x, int y)
        {
            int count = 0;
            if (x == 0 || x == 14)
                count++;
            if (y == 0 || y == 14)
                count++;

            foreach (CityInfo city in cities)
            {
                if (city.PositionX == x - 1 && city.PositionY == y)
                    count++;
                else if (city.PositionX == x && city.PositionY == y - 1)
                    count++;
                else if (city.PositionX == x + 1 && city.PositionY == y)
                    count++;
                else if (city.PositionX == x && city.PositionY == y + 1)
                    count++;
            }

            return count;
        }
        public static int CountBlockingDiagonalNeighboursCount(this IEnumerable<CityInfo> cities, int x, int y)
        {
            int count = 0;
            if (x == 0 || x == 14)
                count += 2;
            if (y == 0 || y == 14)
                count = Math.Min(count + 2, 3);

            foreach (CityInfo city in cities)
            {
                if (city.PositionX == x - 1 && city.PositionY == y - 1)
                    count++;
                else if (city.PositionX == x + 1 && city.PositionY == y - 1)
                    count++;
                else if (city.PositionX == x + 1 && city.PositionY == y + 1)
                    count++;
                else if (city.PositionX == x - 1 && city.PositionY == y + 1)
                    count++;
            }

            return count;
        }
        public static int[,] GetPossibleAttackSum(this WorldInfo world)
        {
            int[,] attackSum = new int[15, 15];
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    attackSum[i, j] = 0;
                }
            }
            foreach (UnitInfo unitInfo in world.GetOthersUnits())
            {
                int movementPoints = UnitStats.TotalMoves[unitInfo.UnitTypeName];
                for (int i = unitInfo.PositionX - movementPoints; i <= unitInfo.PositionX + movementPoints; i++)
                {
                    if (i < 0 || i >= 15)
                        continue;
                    for (int j = unitInfo.PositionY - movementPoints; j <= unitInfo.PositionY + movementPoints; j++)
                    {
                        if (j < 0 || j >= 15)
                            continue;

                        attackSum[i, j] += unitInfo.AttackPower();
                    }
                }
            }
            if (Debugger.IsAttached)
            {
                Debug.WriteLine("Attack table");
                for (int i = 0; i < 15; i++)
                {
                    for (int j = 0; j < 15; j++)
                    {
                        Debug.Write(attackSum[j, i]);
                    }
                    Debug.WriteLine("");
                }
            }

            return attackSum;
        }
        public static double[,] GetDefenseSum(this WorldInfo world)
        {
            double[,] defenseSum = new double[15, 15];
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    defenseSum[i, j] = 0;
                }
            }
            double defenseCoeff = 1;
            if (world.GetMyPlayer().Researched.Contains(Research.kofal)) defenseCoeff =1.4;
            else if (world.GetMyPlayer().Researched.Contains(Research.colopkerites)) defenseCoeff =1.2;

            foreach (UnitInfo unit in world.GetMyUnits())
            {
                defenseSum[unit.PositionX, unit.PositionY] += unit.DefensePower();
            }
            for (int i = 0; i < 15; i++)
			{
			    for (int j = 0; j < 15; j++)
			    {
                    defenseSum[i,j] *= defenseCoeff;
			    }
			}

            return defenseSum;
        }

        #endregion












        public static Point GetEmptyCellNearestTo(this WorldInfo world, CityInfo city, CityInfo fromCity)
        {
            return world.GetEmptyCellNearestTo(city.GetPosition(), fromCity.GetPosition());
        }

        public static Point GetEmptyCellNearestTo(this WorldInfo world, Point position, Point fromPosition)
        {
            return world.GetNeighbouringCells(position).Where(w => !world.IsCityOn(w)).OrderBy(w => w.DistanceTo(fromPosition)).First();
        }

        public static int GetIncome(this WorldInfo world)
        {
            return GetIncome(world, Bicepsz.PlayerNameStatic);
        }
        public static int GetIncome(this WorldInfo world, string name)
        {
            int incomePerCity = 20;
            PlayerInfo p = world.Players.Where(x => x.Name == name).SingleOrDefault();
            if (p == null)
                return -1;
            else
            {
                if (p.Researched.Contains(Research.iras)) incomePerCity += 5;
                if (p.Researched.Contains(Research.birosag)) incomePerCity += 10;
                return incomePerCity * world.Cities.Where(x => x.Owner == name).Count();
            }
        }
        public static int GetIncome(this WorldInfo world, PlayerInfo player)
        {
            return GetIncome(world, player.Name);
        }

        public static CityInfo GetCityOnEdge(this WorldInfo worldInfo)
        {
            CityInfo[] candidates = worldInfo.Cities.Where(w => w.Owner == Bicepsz.PlayerNameStatic && worldInfo.HasEmptyCellInNeighbour(w)).ToArray();
            Random r = new Random();
            return candidates[r.Next(candidates.Length)];
        }

        public static bool HasEmptyCellInNeighbour(this WorldInfo world, CityInfo city)
        {
            return world.HasEmptyCellInNeighbour(city.PositionX, city.PositionY);
        }
        public static bool HasEmptyCellInNeighbour(this WorldInfo world, Point point)
        {
            return world.HasEmptyCellInNeighbour(point.X, point.Y);
        }
        public static bool HasEmptyCellInNeighbour(this WorldInfo world, int x, int y)
        {
            return world.GetNeighbouringCells(x, y).Any(w => !world.IsCityOn(w));
        }

        public static Point[] GetNeighbouringCells(this WorldInfo world, Point position)
        {
            return world.GetNeighbouringCells(position.X, position.Y);
        }
        public static Point[] GetNeighbouringCells(this WorldInfo world, int x, int y)
        {
            return new Point[]
            {
                new Point(x + 1, y - 1),
                new Point(x + 1, y),
                new Point(x + 1, y + 1),
                new Point(x, y - 1),
                new Point(x, y + 1),
                new Point(x - 1, y - 1),
                new Point(x - 1, y),
                new Point(x - 1, y + 1),
            };
        }
        public static Point[] GetAllCells(this WorldInfo world)
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    points.Add(new Point(i, j));
                }
            }
            return points.ToArray();
        }

        public static bool IsCityOn(this WorldInfo worldInfo, Point position)
        {
            return worldInfo.IsCityOn(position.X, position.Y);
        }
        public static bool IsCityOn(this WorldInfo worldInfo, int x, int y)
        {
            return worldInfo.Cities.Any(w => w.PositionX == x && w.PositionY == y);
        }
        public static bool IsMyCityOn(this WorldInfo world, Point cell)
        {
            return world.Cities.Any(w => w.PositionX == cell.X && w.PositionY == cell.Y && w.Owner == Bicepsz.PlayerNameStatic);
        }

        public static bool IsEnemyCityOn(this WorldInfo world, Point cell)
        {
            return world.Cities.Any(w => w.PositionX == cell.X && w.PositionY == cell.Y && w.Owner != Bicepsz.PlayerNameStatic);
        }


        // ------------------

        public static CityInfo GetNearestCityTo(this WorldInfo worldInfo, Point point)
        {
            return worldInfo.GetMyCities().OrderBy(x => x.GetPosition().DistanceTo(point)).First();
        }
        public static Point GetNearestEmptyField(this WorldInfo worldInfo, Point point)
        {
            return worldInfo.GetAllCells().Where(x => !IsCityOn(worldInfo, x)).OrderBy(x => x.DistanceTo(point)).First();
        }
        public static Point GetNearestEmptyField(this WorldInfo worldInfo, Point point, double leastDistance)
        {
            return worldInfo.GetAllCells().Where(x => x.DistanceTo(point) >= leastDistance).Where(x => !IsCityOn(worldInfo, x)).OrderBy(x => x.DistanceTo(point)).First();
        }
        public static Point GetNearestEmptyField(this WorldInfo worldInfo, Point point, double leastDistance, bool even)
        {
            int parity = even ? 0 : 1;
            return worldInfo.GetAllCells()
                .Where(x => x.DistanceTo(point) >= leastDistance && ((x.X + x.Y) % 2 == parity))
                .Where(x => !IsCityOn(worldInfo, x))
                .OrderBy(x => x.DistanceTo(point)).FirstOrDefault();
        }
        public static string GetOwnerNameAtField(this WorldInfo world, Point point)
        {
            var city = world.Cities.Where(x => (x.PositionX == point.X && x.PositionY == point.Y)).SingleOrDefault();
            if (city != null)
                return city.Owner;
            else return null;
        }
        public static double GetCoverageFactor(this WorldInfo world, Point point)
        {
            double factor = 0;
            if (IsCityOn(world, point))
            {
                string ownerName = GetOwnerNameAtField(world, point);
                foreach (var neighbour in GetNeighbouringCells(world, point))
                {
                    if (ownerName == GetOwnerNameAtField(world, point))
                        factor++;
                }
                return factor;
            }
            else
                return -1;
        }
        public static Point[] GetUndefendedCityInSight(this WorldInfo world, UnitInfo unit)
        {
            return world.GetAllCells()
                .Where(x => x.DistanceTo(unit.GetPosition()) < Math.Sqrt(Math.Pow(unit.TotalMoves(), 2) + Math.Pow(unit.TotalMoves(), 2))
                    && (world.GetOwnerNameAtField(x) == null || (world.GetOwnerNameAtField(x) != Bicepsz.PlayerNameStatic && !IsCityOn(world, x))))
                .ToArray();
        }

        public static int GetIncome(this WorldInfo world, string name, bool tryIras, bool tryBirosag)
        {
            if (tryBirosag)
                tryIras = tryBirosag;
            int incomePerCity = 20;
            PlayerInfo p = world.Players.Where(x => x.Name == name).SingleOrDefault();
            if (p == null)
                throw new Exception();
            else
            {
                if (p.Researched.Contains(Research.iras)) incomePerCity += 5;
                else if (tryIras) incomePerCity += 5;
                if (p.Researched.Contains(Research.birosag)) incomePerCity += 10;
                else if (tryBirosag) incomePerCity += 10;
                return incomePerCity * world.Cities.Where(x => x.Owner == name).Count();
            }
        }

        public static int GetIncome(this WorldInfo world, int additionalvillages)
        {
            int moneyPerVillage = world.GetMyPlayer().Researched.Contains(Research.birosag) ? 35 : world.GetMyPlayer().Researched.Contains(Research.iras) ? 25 : 20;
            return GetIncome(world) + additionalvillages * moneyPerVillage;
        }
        public static bool IsEnemyUnitOn(this WorldInfo world, Point point)
        {
            return world.Units.Where(w => w.Owner != Bicepsz.PlayerNameStatic).Any(w => w.PositionX == point.X && w.PositionY == point.Y);
        }
        public static bool IsUnitOn(this WorldInfo world, Point point, string owner = null)
        {
            return IsUnitOn(world, point.X, point.Y, owner);
        }
        public static bool IsUnitOn(this WorldInfo world, int x, int y, string owner = null)
        {
            if (owner == null)
                return world.Units.Any(w => w.PositionX == x && w.PositionY == y);
            else
                return world.Units.Where(w => w.Owner == owner).Any(w => w.PositionX == x && w.PositionY == y);
        }
        public static UnitInfo GetNearestUnit(this WorldInfo world, Point p, string owner = null)
        {
            if (owner == null)
            {
                return world.Units.OrderBy(w => p.DistanceTo(w.GetPosition())).FirstOrDefault();
            }
            else
                return world.Units.Where(w => w.Owner == owner).OrderBy(w => p.DistanceTo(w.GetPosition())).FirstOrDefault();
        }
        public static UnitInfo GetMyNearestUnit(this WorldInfo world, Point p)
        {
            return GetNearestUnit(world, p, Bicepsz.PlayerNameStatic);
        }
    }
}
