using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2021
{
    [TestClass]
    public class Day8 : AoCTestClass
    {
        [TestMethod]
        public void Part1()
        {
            var dayInput = DayInput();
            var stringReader = new StringReader(dayInput);
            string? line;
            var easyDigits = 0;
            while ((line = stringReader.ReadLine()) != null)
            {
                var fourDigitOutput = line.Split(" | ")[1];
                easyDigits += fourDigitOutput.Split(' ').Count(s => s.Length is <= 4 or 7);
            }

            TestContext.WriteLine($"Easy digits: {easyDigits}");
        }

        [TestMethod]
        public void Part2()
        {
            var dayInput = DayInput();
            var stringReader = new StringReader(dayInput);
            string? line;
            var sum = 0;
            while ((line = stringReader.ReadLine()) != null)
            {
                var strings = line.Split(" | ");
                var patterns = strings[0].Split(' ').OrderBy(s => s.Length).ToArray();
                var numbers = new HashSet<char>[10];
                numbers[1] = patterns[0].ToHashSet();
                numbers[7] = patterns[1].ToHashSet();
                numbers[4] = patterns[2].ToHashSet();
                numbers[8] = patterns[9].ToHashSet();
                var twofivethree = patterns[3..6].OrderBy(p => p.Intersect(numbers[1]).Count()).ToArray();
                numbers[3] = twofivethree[2].ToHashSet();
                numbers[5] = twofivethree.First(p => numbers[4].Intersect(p).Count() == 3).ToHashSet();
                numbers[2] = twofivethree.First(p => numbers[4].Intersect(p).Count() == 2).ToHashSet();
                var zerosixnine = patterns[6..9].OrderBy(p => p.Intersect(numbers[3]).Count()).ToArray();
                numbers[9] = zerosixnine[2].ToHashSet();
                numbers[0] = zerosixnine.First(p => numbers[1].Intersect(p).Count() == 2).ToHashSet();
                numbers[6] = zerosixnine.First(p => numbers[1].Intersect(p).Count() == 1).ToHashSet();


                sum += GetOutput(strings, numbers);
            }
            TestContext.WriteLine($"Sum: {sum}");
        }

        private int GetOutput(string[] strings, HashSet<char>[] numbers)
        {
            var multiplier = 1000;
            var output = 0;
            for (int i = 0; i < 4; i++)
            {
                output += multiplier * GetDigit(strings[1].Split(' ')[i], numbers);
                multiplier /= 10;
            }

            return output;
        }

        private int GetDigit(string s, HashSet<char>[] numbers)
        {
            for (var i = 0; i < 10; i++)
            {
                if (numbers[i].SetEquals(s))
                    return i;
            }

            throw new Exception("Unexpected input");
        }

        private static string TestInput2() => @"acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf";
        private static string TestInput() => @"be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe
edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc
fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg
fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb
aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea
fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb
dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe
bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef
egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb
gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce";

    }
}
