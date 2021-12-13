using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2021
{
    [TestClass]
    public class Day13 : AoCTestClass
    {
        [TestMethod]
        public void Part1()
        {
            var dayInput = DayInput();
            var stringReader = new StringReader(dayInput);
            string? line;
            var paper = new HashSet<(int x, int y)>();
            while ((line = stringReader.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                    break;

                var dot = line.Split(',').Select(int.Parse).ToArray();
                paper.Add((dot[0], dot[1]));
            }

            var fold = stringReader.ReadLine();
            var match = Regex.Match(fold, @"(x|y)=(\d+)");
            var dir = match.Groups[1].Value;
            var foldLine = int.Parse(match.Groups[2].Value);

            var foldedDots = paper.Where(coord => (dir == "x" ? coord.x : coord.y) > foldLine).ToHashSet();
            paper.ExceptWith(foldedDots);
            foreach (var (x, y) in foldedDots)
            {
                paper.Add(dir == "x"
                    ? (2 * foldLine - x, y)
                    : (x, 2 * foldLine - y));
            }

            TestContext.WriteLine($"Dots: {paper.Count}");
        }

        [TestMethod]
        public void Part2()
        {
            var dayInput = DayInput();
            var stringReader = new StringReader(dayInput);
            string? line;
            var paper = new HashSet<(int x, int y)>();
            while ((line = stringReader.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                    break;

                var dot = line.Split(',').Select(int.Parse).ToArray();
                paper.Add((dot[0], dot[1]));
            }

            while ((line = stringReader.ReadLine()) != null)
            {
                var match = Regex.Match(line, @"(x|y)=(\d+)");
                var dir = match.Groups[1].Value;
                var foldLine = int.Parse(match.Groups[2].Value);

                var foldedDots = paper.Where(coord => (dir == "x" ? coord.x : coord.y) > foldLine).ToHashSet();
                paper.ExceptWith(foldedDots);
                foreach (var (x, y) in foldedDots)
                {
                    paper.Add(dir == "x"
                        ? (2 * foldLine - x, y)
                        : (x, 2 * foldLine - y));
                }
            }

            Print(paper);
        }

        private void Print(HashSet<(int x, int y)> paper)
        {
            var (minx, miny) = paper.Min();
            var (maxx, maxy) = paper.Max();
            for (var y = miny; y <= maxy; y++)
            {
                for (var x = minx; x <= maxx; x++)
                {
                    TestContext.Write(paper.Contains((x, y)) ? "#" : " ");
                }

                TestContext.WriteLine("");
            }
        }

        private static string TestInput() => @"6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5";

    }
}
