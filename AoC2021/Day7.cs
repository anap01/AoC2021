using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2021
{
    [TestClass]
    public class Day7 : AoCTestClass
    {
        [TestMethod]
        public void Part1()
        {
            var dayInput = DayInput();
            var positions = dayInput.Split(',').Select(int.Parse).ToArray();
            var min = positions.Min();
            var max = positions.Max();
            var result = int.MaxValue;
            for (int i = min; i <= max; i++)
            {
                var test = positions.Select(x => Math.Abs(x - i)).Sum();
                if (test < result)
                    result = test;
            }
            TestContext.WriteLine($"Result: {result}");
        }

        [TestMethod]
        public void Part2()
        {
            var dayInput = DayInput();
            var positions = dayInput.Split(',').Select(int.Parse).ToArray();
            var min = positions.Min();
            var max = positions.Max();
            var result = int.MaxValue;
            for (int i = min; i <= max; i++)
            {
                var test = positions.Select(x =>
                {
                    var distance = Math.Abs(x - i);
                    return distance*(distance + 1)/2;
                }).Sum();
                if (test < result)
                    result = test;
            }
            TestContext.WriteLine($"Result: {result}");        }

        private static string TestInput() => @"16,1,2,0,4,2,7,1,2,14";
    }
}
