using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2021
{
    [TestClass]
    public class Day3 : AoCTestClass
    {
        [TestMethod]
        public void Part1()
        {
            var dayInput = DayInput();

            var stringReader = new StringReader(dayInput);
            var line = stringReader.ReadLine();
            var result = new int[line.Length];
            do
            {
                foreach (var (c, i) in line.Select((c, i) => (c, i)))
                {
                    if (c == '1')
                        result[i] += 1;
                }
            } while ((line = stringReader.ReadLine()) != null);

            var noRows = dayInput.Count(c => c == '\n') + 1;
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] > noRows / 2)
                    result[i] = 1;
                else
                    result[i] = 0;
            }

            var gamma = Convert.ToInt32(string.Join("", result.Select(i => $"{i}")), 2);
            var mask = Convert.ToInt32(new string(Enumerable.Repeat('1', result.Length).ToArray()), 2);
            var epsilon = ~gamma & mask;
            TestContext.WriteLine($"gamma: {gamma}");
            TestContext.WriteLine($"epsilon: {epsilon}");
            TestContext.WriteLine($"Result: {gamma * epsilon}");
        }


        [TestMethod]
        public void Part2()
        {
            var dayInput = DayInput();

            var strings = dayInput.Split(new []{'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries).ToList();
            var i = 0;

            while (strings.Count > 1)
            {
                var count = strings.Count(s => s[i] == '1');
                if (count >= strings.Count / 2.0)
                    strings.RemoveAll(s => s[i] == '0');
                else
                    strings.RemoveAll(s => s[i] == '1');
                i++;
            }

            var oxygen = strings[0];

            strings = dayInput.Split(new []{'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries).ToList();
            i = 0;
            while (strings.Count > 1)
            {
                var count = strings.Count(s => s[i] == '1');
                if (count >= strings.Count / 2.0)
                    strings.RemoveAll(s => s[i] == '1');
                else
                    strings.RemoveAll(s => s[i] == '0');
                i++;
            }

            var co2 = strings[0];
            TestContext.WriteLine($"oxygen: {oxygen}");
            TestContext.WriteLine($"co2: {co2}");
            TestContext.WriteLine($"Result: {Convert.ToInt32(oxygen, 2) * Convert.ToInt32(co2, 2)}");

        }
    }
}
