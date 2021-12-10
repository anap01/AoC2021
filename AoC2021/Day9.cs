using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2021
{
    [TestClass]
    public class Day9 : AoCTestClass
    {
        [TestMethod]
        public void Part1()
        {
            var dayInput = DayInput();
            var heightmap = dayInput.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(row => row.Select(c => int.Parse(c.ToString())).ToArray())
                .ToArray();
            var riskLevel = 0;
            for (var i = 0; i < heightmap.Length; i++)
            {
                for (var j = 0; j < heightmap[i].Length; j++)
                {
                    var low = true;
                    foreach (var neighbour in Neighbours(i, j, heightmap))
                    {
                        if (heightmap[i][j] >= neighbour)
                            low = false;
                    }

                    if (low)
                        riskLevel += heightmap[i][j] + 1;
                }
            }
            TestContext.WriteLine($"Risk level: {riskLevel}");
        }

        private IEnumerable<int> Neighbours(int row, int col, int[][] heightmap)
        {
            if (row > 0)
                yield return heightmap[row - 1][col];
            if (row < heightmap.Length - 1)
                yield return heightmap[row + 1][col];
            if (col > 0)
                yield return heightmap[row][col - 1];
            if (col < heightmap[row].Length - 1)
                yield return heightmap[row][col + 1];
        }

        [TestMethod]
        public void Part2()
        {
            var dayInput = DayInput();
            var heightmap = dayInput.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(row => row.Select(c => int.Parse(c.ToString())).ToArray())
                .ToArray();
            var basins = new HashSet<HashSet<(int, int)>?>();
            for (var i = 0; i < heightmap.Length; i++)
            {
                HashSet<(int, int)>? currentBasin = null;
                for (var j = 0; j < heightmap[i].Length; j++)
                {
                    if (heightmap[i][j] == 9)
                    {
                        if (currentBasin != null && basins.Contains(currentBasin))
                        {
                            currentBasin = null;
                            continue;
                        }

                        if (currentBasin != null)
                        {
                            MergeBasins(basins, currentBasin);
                            currentBasin = null;
                        }
                        continue;
                    }

                    if (currentBasin == null && HasNeighbour(i, j, basins, out currentBasin) == false)
                    {
                        currentBasin = new HashSet<(int, int)>();
                    }
                    currentBasin.Add((i, j));
                }

                if (currentBasin != null)
                    MergeBasins(basins, currentBasin);
            }

            var result = basins.Select(b => b.Count).OrderByDescending(c => c).Take(3).Aggregate(1L, (prev, current) => prev * current);
            TestContext.WriteLine($"result: {result}");
        }

        private void MergeBasins(HashSet<HashSet<(int, int)>?>? basins, HashSet<(int, int)>? currentBasin)
        {
            foreach (var basin in basins.ToList())
            {
                if (currentBasin.SelectMany(p => GetNeighbours(p.Item1, p.Item2)).ToHashSet()
                    .Overlaps(basin))
                {
                    basins.Remove(basin);
                    foreach (var valueTuple in basin)
                    {
                        currentBasin.Add(valueTuple);
                    }
                }
            }

            basins.Add(currentBasin);
        }

        private bool HasNeighbour(int row, int col, HashSet<HashSet<(int, int)>?> basins, out HashSet<(int, int)>? currentBasin)
        {
            currentBasin = null;
            foreach (var basin in basins)
            {
                if (HasNeighbour(row, col, basin))
                {
                    currentBasin = basin;
                    return true;
                }
            }

            return false;
        }

        private bool HasNeighbour(int row, int col, HashSet<(int, int)>? basins)
        {
            return basins.Overlaps(GetNeighbours(row, col));
        }

        private IEnumerable<(int, int)> GetNeighbours(int row, int col)
        {
                yield return (row - 1, col);
                yield return (row + 1, col);
                yield return (row, col - 1);
                yield return (row, col + 1);
        }

        private static string TestInput() => @"2199943210
3987894921
9856789892
8767896789
9899965678";

    }
}
