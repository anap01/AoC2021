using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2021
{
    [TestClass]
    public class Day12 : AoCTestClass
    {
        [TestMethod]
        public void Part1()
        {
            var dayInput = DayInput();
            var stringReader = new StringReader(dayInput);
            string? line;
            var caves = new Dictionary<string, Cave>();
            while ((line = stringReader.ReadLine()) != null)
            {
                var strings = line.Split('-');
                var cave1Name = strings[0];
                if (caves.TryGetValue(cave1Name, out var cave1) == false)
                {
                    cave1 = new Cave(cave1Name);
                    caves[cave1Name] = cave1;
                }
                var cave2Name = strings[1];
                if (caves.TryGetValue(cave2Name, out var cave2) == false)
                {
                    cave2 = new Cave(cave2Name);
                    caves[cave2Name] = cave2;
                }

                cave1.ConnectedCaves.Add(cave2);
                cave2.ConnectedCaves.Add(cave1);
            }

            var start = caves["start"];
            var allPaths = FindPaths(start).Where(p => p.Last().Name == "end").ToList();
            TestContext.WriteLine($"Count: {allPaths.Count}");
        }

        private IEnumerable<IEnumerable<Cave>> FindPaths(Cave cave, HashSet<string>? visited = null)
        {
            visited ??= new HashSet<string>();

            if (cave.Name == "end")
            {
                yield return new List<Cave> { cave };
                yield break;
            }

            if (cave.Name == cave.Name.ToLower())
                visited.Add(cave.Name);

            foreach (var connectedCave in cave.ConnectedCaves.Where(c => visited.Contains(c.Name) == false))
            {
                foreach (var path in FindPaths(connectedCave, new HashSet<string>(visited)))
                {
                    yield return path.Prepend(cave).ToList();
                }
            }

            yield return new List<Cave> { cave };
        }

        [TestMethod]
        public void Part2()
        {
            var dayInput = DayInput();
            var stringReader = new StringReader(dayInput);
            string? line;
            var caves = new Dictionary<string, Cave>();
            while ((line = stringReader.ReadLine()) != null)
            {
                var strings = line.Split('-');
                var cave1Name = strings[0];
                if (caves.TryGetValue(cave1Name, out var cave1) == false)
                {
                    cave1 = new Cave(cave1Name);
                    caves[cave1Name] = cave1;
                }
                var cave2Name = strings[1];
                if (caves.TryGetValue(cave2Name, out var cave2) == false)
                {
                    cave2 = new Cave(cave2Name);
                    caves[cave2Name] = cave2;
                }

                cave1.ConnectedCaves.Add(cave2);
                cave2.ConnectedCaves.Add(cave1);
            }

            var start = caves["start"];
            var allPaths = FindPaths2(start).Where(p => p.Last().Name == "end").ToList();
            TestContext.WriteLine($"Count: {allPaths.Count}");
        }

        private IEnumerable<IEnumerable<Cave>> FindPaths2(Cave cave, List<string>? visited = null)
        {
            visited ??= new List<string>();

            if (cave.Name == "end")
            {
                yield return new List<Cave> { cave };
                yield break;
            }

            if (cave.Name == cave.Name.ToLower())
                visited.Add(cave.Name);

            bool Predicate(Cave c)
            {
                if (c.Name == "start")
                    return false;
                var groupByCaveName = visited.GroupBy(s => s).ToList();
                var smallCaveVisitedTwice = groupByCaveName.Any(g => g.Count() > 1);
                var currentCount = groupByCaveName.FirstOrDefault(g => g.Key == c.Name)?.Count() ?? 0;
                return smallCaveVisitedTwice ? currentCount < 1 : currentCount < 2;
            }

            foreach (var connectedCave in cave.ConnectedCaves.Where(Predicate))
            {
                foreach (var path in FindPaths2(connectedCave, new List<string>(visited)))
                {
                    yield return path.Prepend(cave).ToList();
                }
            }

            yield return new List<Cave> { cave };
        }


        private static string TestInput() => @"start-A
start-b
A-c
A-b
b-d
A-end
b-end";

        private static string TestInput2() => @"dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc";
    }


    [DebuggerDisplay("{Name}")]
    public class Cave
    {
        public string Name { get; }

        public HashSet<Cave> ConnectedCaves { get; set; } = new();

        public Cave(string name)
        {
            Name = name;
        }
    }
}
