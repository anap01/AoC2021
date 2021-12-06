using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2021
{
    [TestClass]
    public class Day5 : AoCTestClass
    {
        [TestMethod]
        public void Part1()
        {
            var dayInput = DayInput();
            var stringReader = new StringReader(dayInput);
            string? line;
            var field = new Dictionary<(int, int), int>();
            while ((line = stringReader.ReadLine()) != null)
            {
                var numbers = line.Split(" -> ").Select(coord =>
                {
                    var coords = coord.Split(',').Select(int.Parse).ToArray();
                    return (coords[0], coords[1]);
                }).ToArray();
                var (x1, y1) = numbers[0];
                var (x2, y2) = numbers[1];
                if (x1 != x2 && y1 != y2)
                    continue;
                if (x1 > x2)
                    (x1, x2) = (x2, x1);
                if (y1 > y2)
                    (y1, y2) = (y2, y1);

                for (var x = x1; x <= x2; x++)
                {
                    for (var y = y1; y <= y2; y++)
                    {
                        if (field.TryGetValue((x, y), out var count))
                        {
                            field[(x, y)] = count + 1;
                        }
                        else
                        {
                            field[(x, y)] = 1;
                        }
                    }
                }
            }

            var points = field.Count(kvp => kvp.Value > 1);
            TestContext.WriteLine($"Points: {points}");
        }

        [TestMethod]
        public void Part2()
        {
            var dayInput = DayInput();
            var stringReader = new StringReader(dayInput);
            string? line;
            var field = new Dictionary<(int, int), int>();
            while ((line = stringReader.ReadLine()) != null)
            {
                var numbers = line.Split(" -> ").Select(coord =>
                {
                    var coords = coord.Split(',').Select(int.Parse).ToArray();
                    return (coords[0], coords[1]);
                }).ToArray();
                var (x1, y1) = numbers[0];
                var (x2, y2) = numbers[1];

                var steps = Math.Max(Math.Abs(x2 - x1), Math.Abs(y2 - y1));
                var dx = (x2 - x1) / steps;
                var dy = (y2 - y1) / steps;
                for (var i = 0; i <= steps; i++)
                {
                    var x = x1 + i * dx;
                    var y = y1 + i * dy;
                    if (field.TryGetValue((x, y), out var count))
                    {
                        field[(x, y)] = count + 1;
                    }
                    else
                    {
                        field[(x, y)] = 1;
                    }
                }
            }

            var points = field.Count(kvp => kvp.Value > 1);
            TestContext.WriteLine($"Points: {points}");        }

        private static string TestInput() => @"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2";
    }
}
