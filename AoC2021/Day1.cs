using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2021
{
    [TestClass]
    public class Day1 : AoCTestClass
    {
        [TestMethod]
        public void Part1()
        {
            var dayInput = DayInput();
            // Iterate over lines
            var stringReader = new StringReader(dayInput);
            string line;
            var previous = int.MaxValue;
            var increase = 0;
            while ((line = stringReader.ReadLine()) != null)
            {
                var current = int.Parse(line);
                if (current > previous)
                    increase++;
                previous = current;
            }
            TestContext.WriteLine($"{increase}");

        }

        [TestMethod]
        public void Part2()
        {
            var dayInput = DayInput();
            var numbers = dayInput.Split(new[] {'\n'}, StringSplitOptions.None).Select(int.Parse).ToArray();

            // Iterate over lines
            var increase = 0;
            for (var i = 4; i <= numbers.Length; i++)
            {
                var first = numbers[(i - 4)..(i - 1)].Sum();
                var second = numbers[(i - 3)..i].Sum();
                if (second > first)
                    increase++;
            }
            TestContext.WriteLine($"{increase}");

        }
    }
}
