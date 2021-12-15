using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2021
{
    [TestClass]
    public class Day15 : AoCTestClass
    {
        [TestMethod]
        public void Part1()
        {
            var dayInput = DayInput();
            var cavern = dayInput.Split(new []{'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries).Select(row => row.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
            var start = (0, 0);
            var openSet = new Dictionary<(int, int), int>();
            openSet[start] = 0;
            var visited = new HashSet<(int, int)>();
            while (openSet.Count > 0)
            {
                var minScore = openSet.Values.Min();
                var (node, score) = openSet.First(kvp => kvp.Value == minScore);
                if (node == (cavern[0].Length - 1, cavern.Length - 1))
                {
                    TestContext.WriteLine($"Result: {score}");
                }

                openSet.Remove(node);
                visited.Add(node);
                foreach (var neighbour in GetNeighbours(node, cavern).Where((n => visited.Contains(n) == false)))
                {
                    var testScore = score + cavern[neighbour.Item2][neighbour.Item1];
                    if (openSet.TryGetValue(neighbour, out var currentScore))
                    {
                        if (testScore < currentScore)
                            openSet[neighbour] = testScore;
                    }
                    else
                    {
                        openSet[neighbour] = testScore;
                    }
                }
            }
        }

        [TestMethod]
        public void Part2()
        {
            var dayInput = DayInput();
            var cavern = dayInput.Split(new []{'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries).Select(row => row.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
            var start = (0, 0);
            var openSet = new Dictionary<(int, int), long>();
            openSet[start] = 0;
            var visited = new HashSet<(int, int)>();
            while (openSet.Count > 0)
            {
                var minScore = openSet.Values.Min();
                var (node, score) = openSet.First(kvp => kvp.Value == minScore);
                if (node == (5 * cavern[node.Item2 % cavern.Length].Length - 1, 5 * cavern.Length - 1))
                {
                    TestContext.WriteLine($"Result: {score}");
                }

                openSet.Remove(node);
                visited.Add(node);
                foreach (var neighbour in GetNeighbours2(node, cavern).Where((n => visited.Contains(n) == false)))
                {
                    var xFactor = neighbour.Item1 / cavern[neighbour.Item1 % cavern.Length].Length;
                    var yFactor = neighbour.Item2 / cavern.Length;
                    var scoreAtNode = xFactor + yFactor + cavern[neighbour.Item2 % cavern.Length][neighbour.Item1 % cavern[0].Length];
                    if (scoreAtNode > 9)
                        scoreAtNode -= 9;
                    var testScore = score + scoreAtNode;
                    if (openSet.TryGetValue(neighbour, out var currentScore))
                    {
                        if (testScore < currentScore)
                            openSet[neighbour] = testScore;
                    }
                    else
                    {
                        openSet[neighbour] = testScore;
                    }
                }
            }
        }

        private IEnumerable<(int, int)> GetNeighbours((int, int) node, int[][] cavern)
        {
            if (node.Item1 > 0)
                yield return (node.Item1 - 1, node.Item2);
            if (node.Item1 < cavern[node.Item2].Length - 1)
                yield return (node.Item1 + 1, node.Item2);
            if (node.Item2 > 0)
                yield return (node.Item1, node.Item2 - 1);
            if (node.Item2 < cavern.Length - 1)
                yield return (node.Item1, node.Item2 + 1);
        }

        private IEnumerable<(int, int)> GetNeighbours2((int, int) node, int[][] cavern)
        {
            if (node.Item1 > 0)
                yield return (node.Item1 - 1, node.Item2);
            if (node.Item1 < 5 * cavern[node.Item2 % cavern.Length].Length - 1)
                yield return (node.Item1 + 1, node.Item2);
            if (node.Item2 > 0)
                yield return (node.Item1, node.Item2 - 1);
            if (node.Item2 < 5 * cavern.Length - 1)
                yield return (node.Item1, node.Item2 + 1);
        }

        private static string TestInput() => @"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581";

    }
}
