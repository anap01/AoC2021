using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2021
{
    [TestClass]
    public class Day11 : AoCTestClass
    {
        public static Octopus[][] m_octopuses;

        [TestMethod]
        public void Part1()
        {
            var dayInput = DayInput();
            var rows = dayInput.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            m_octopuses = rows.Select((row, y) => row.Select<char, Octopus>((c, x) => new Octopus(x, y, int.Parse(c.ToString()))).ToArray())
                .ToArray();
            for (int i = 0; i < 100; i++)
            {
                Step();
            }

            DebugOutput();
            TestContext.WriteLine($"Flashes: {Octopus.Flashes}");
        }

        [TestMethod]
        public void Part2()
        {
            var dayInput = DayInput();
            var rows = dayInput.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            m_octopuses = rows.Select((row, y) => row.Select<char, Octopus>((c, x) => new Octopus(x, y, int.Parse(c.ToString()))).ToArray())
                .ToArray();
            for (var i = 1; i < int.MaxValue; i++)
            {
                Step();
                if (m_octopuses.SelectMany(row => row).Any(o => o.Energy != 0))
                    continue;

                TestContext.WriteLine($"Iteration: {i}");
                break;
            }
        }

        private void Step()
        {
            foreach (var octopus in m_octopuses.SelectMany(row => row))
            {
                octopus.Step();
            }

            while (m_octopuses.SelectMany(row => row).Any(o => o.Flash()))
                ;

            foreach (var octopus in m_octopuses.SelectMany(row => row))
            {
                octopus.Reset();
            }
        }

        private void DebugOutput()
        {
            foreach (var row in m_octopuses)
            {
                foreach (var octopus in row)
                {
                    TestContext.Write(octopus.ToString());
                }

                TestContext.WriteLine("");
            }
            TestContext.WriteLine("");
        }

        private static string TestInput() => @"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526";

        private static string TestInput2() => @"11111
19991
19191
19991
11111";

    }

    public class Octopus
    {
        private readonly int m_x;
        private readonly int m_y;
        public static long Flashes;

        public Octopus(int x, int y, int energy)
        {
            m_x = x;
            m_y = y;
            Energy = energy;
        }

        public void Step()
        {
            Energy++;
        }

        public bool Flash() {
            if (Energy < 10)
                return false;

            Flashes++;
            Energy = int.MinValue;

            foreach (var neighbour in Neighbours)
            {
                neighbour.Step();
            }

            return true;
        }

        private IEnumerable<Octopus> Neighbours
        {
            get
            {
                for (var y = Math.Max(0, m_y - 1); y <= Math.Min(m_y + 1, Day11.m_octopuses.Length - 1); y++)
                {
                    for (var x = Math.Max(0, m_x - 1); x <= Math.Min(m_x + 1, Day11.m_octopuses[m_y].Length - 1); x++)
                    {
                        if ((x, y) == (m_x, m_y))
                            continue;

                        yield return Day11.m_octopuses[y][x];
                    }
                }
            }
        }

        public int Energy { get; private set; }

        public override string ToString()
        {
            return Energy.ToString();
        }

        public void Reset()
        {
            if (Energy < 0)
                Energy = 0;
        }
    }
}
