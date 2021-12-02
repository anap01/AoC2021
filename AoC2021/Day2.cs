using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2021
{
    [TestClass]
    public class Day2 : AoCTestClass
    {
        [TestMethod]
        public void Part1()
        {
            var dayInput = DayInput();
            var stringReader = new StringReader(dayInput);
            string? line;
            var regex = new Regex(@"(.+) (\d+)");
            var (pos, depth) = (0, 0);
            while ((line = stringReader.ReadLine()) != null)
            {
                var match = regex.Match(line);
                switch (match.Groups[1].Value)
                {
                    case "forward":
                        pos += int.Parse(match.Groups[2].Value);
                        break;
                    case "up":
                        depth -= int.Parse(match.Groups[2].Value);
                        break;
                    case "down":
                        depth += int.Parse(match.Groups[2].Value);
                        break;
                }
            }

            TestContext.WriteLine($"pos: {pos}");
            TestContext.WriteLine($"depth: {depth}");
            TestContext.WriteLine($"result: {depth*pos}");
        }

        [TestMethod]
        public void Part2()
        {
            var dayInput = DayInput();
            var stringReader = new StringReader(dayInput);
            string? line;
            var regex = new Regex(@"(.+) (\d+)");
            var (pos, depth) = (0, 0);
            var aim = 0;
            while ((line = stringReader.ReadLine()) != null)
            {
                var match = regex.Match(line);
                var value = int.Parse(match.Groups[2].Value);
                switch (match.Groups[1].Value)
                {
                    case "forward":
                        pos += value;
                        depth += value * aim;
                        break;
                    case "up":
                        aim -= value;
                        break;
                    case "down":
                        aim += value;
                        break;
                }
            }

            TestContext.WriteLine($"pos: {pos}");
            TestContext.WriteLine($"depth: {depth}");
            TestContext.WriteLine($"result: {depth*pos}");
        }
    }
}
