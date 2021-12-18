using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2021
{
    [TestClass]
    public class Day17 : AoCTestClass
    {
        [TestMethod]
        public void Part1()
        {
            var dayInput = DayInput();
            var match = Regex.Match(dayInput, @"x=(-?\d+)\.\.(-?\d+), y=(-?\d+)\.\.(-?\d+)");
            var (x1, x2) = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
            var (y1, y2) = (int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));

            var yMax = 0;
            for (var x = 1; x < 1000; x++)
            for (var y = 1; y < 1000; y++)
            {
                var yTop = GetYForHit((x, y), (x1, x2), (y1, y2));
                yMax = Math.Max(yTop, yMax);
            }
            TestContext.WriteLine($"yMax: {yMax}");
        }

        [TestMethod]
        public void Part2()
        {
            var dayInput = DayInput();
            var match = Regex.Match(dayInput, @"x=(-?\d+)\.\.(-?\d+), y=(-?\d+)\.\.(-?\d+)");
            var (x1, x2) = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
            var (y1, y2) = (int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));

            var valueCount = 0;
            for (var x = -1000; x < 1000; x++)
            for (var y = -1000; y < 1000; y++)
            {
                var yTop = GetYForHit((x, y), (x1, x2), (y1, y2));
                if (yTop != int.MinValue)
                    valueCount++;
            }
            TestContext.WriteLine($"ValueCount: {valueCount}");
        }

        private int GetYForHit((int x, int y) velocity, (int x1, int x2) xRange, (int y1, int y2) yRange)
        {
            var (x, y) = (0, 0);
            var yMax = 0;
            var hit = false;
            var (x1, x2) = xRange;
            var (y1, y2) = yRange;
            var (vx, vy) = velocity;
            while (x < x2 && y > y1)
            {
                x += vx;
                y += vy;
                yMax = Math.Max(y, yMax);
                if (x >= x1 && x <= x2 && y >= y1 && y <= y2)
                    hit = true;
                vx = Math.Max(0, vx - 1);
                vy -= 1;
            }

            return hit ? yMax : int.MinValue;
        }

        private static string TestInput() => @"target area: x=20..30, y=-10..-5";

    }
}
