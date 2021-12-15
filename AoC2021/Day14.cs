using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2021
{
    [TestClass]
    public class Day14 : AoCTestClass
    {
        [TestMethod]
        public void Part1()
        {
            var dayInput = DayInput();
            var stringReader = new StringReader(dayInput);
            string? line;
            var input = stringReader.ReadLine();
            stringReader.ReadLine();
            var rules = new Dictionary<string, char>();
            while ((line = stringReader.ReadLine()) != null)
            {
                var rule = line.Split(" -> ");
                rules.Add(rule[0], rule[1][0]);
            }

            IEnumerable<char> polymer = input!;
            for (var i = 0; i < 10; i++)
            {
                polymer = Step(polymer, rules);
            }

            var orderedEnumerable = polymer.GroupBy(c => c).OrderBy(g => g.Count()).ToList();
            var result = orderedEnumerable.Last().Count() - orderedEnumerable.First().Count();
            TestContext.WriteLine($"Result: {result}");
        }

        private static IEnumerable<char> Step(IEnumerable<char> input, IReadOnlyDictionary<string, char> rules)
        {
            using var charEnumerator = input.GetEnumerator();
            charEnumerator.MoveNext();
            var first = charEnumerator.Current;
            while (charEnumerator.MoveNext())
            {
                var second = charEnumerator.Current;
                yield return first;
                yield return rules[new string(new[] { first, second })];
                first = second;
            }

            yield return first;
        }

        [TestMethod]
        public void Part2()
        {
            var dayInput = DayInput();
            var stringReader = new StringReader(dayInput);
            string? line;
            var input = stringReader.ReadLine();
            stringReader.ReadLine();
            var rules = new Dictionary<(char, char, int), Dictionary<char, long>>();
            while ((line = stringReader.ReadLine()) != null)
            {
                var rule = line.Split(" -> ");
                rules.Add((rule[0][0], rule[0][1], 1), new Dictionary<char, long> { { rule[1][0], 1L } });
            }

            var charCount = new Dictionary<char, long>();
            foreach (var c in input)
            {
                InsertOrAdd(c, 1, charCount);
            }

            for (int i = 0; i < input.Length - 1; i++)
            {
                foreach (var kvp in GrowPolymer(input[i], input[i + 1], 40, rules))
                {
                    InsertOrAdd(kvp.Key, kvp.Value, charCount);
                }
            }
            TestContext.WriteLine($"Result: {charCount.Values.Max() - charCount.Values.Min()}");
        }

        private Dictionary<char, long> GrowPolymer(char first, char second, int steps,
            Dictionary<(char, char, int), Dictionary<char, long>> rules)
        {
            if (rules.TryGetValue((first, second, steps), out var result))
                return result;

            result = new Dictionary<char, long>();

            foreach (var c in GrowPolymer(first, rules[(first, second, 1)].Keys.First(), steps - 1, rules))
            {
                InsertOrAdd(c.Key, c.Value, result);
            }

            InsertOrAdd(rules[(first, second, 1)].Keys.First(), 1, result);

            foreach (var c in GrowPolymer(rules[(first, second, 1)].Keys.First(), second, steps - 1, rules))
            {
                InsertOrAdd(c.Key, c.Value, result);
            }

            rules[(first, second, steps)] = result;

            return result;
        }

        private Dictionary<char, long> CountPolymers(string input, int depth, Dictionary<string, char> rules)
        {
            var dictionary = new Dictionary<char, long>();
            if (depth == 0)
            {
                InsertOrAdd(input[0], 1, dictionary);
                InsertOrAdd(rules[input], 1, dictionary);
                InsertOrAdd(input[1], 1, dictionary);
                return dictionary;
            }

            for (var i = 0; i < input.Length - 1; i++)
            {
                foreach (var (key, value) in CountPolymers(input[i..(i + 2)], depth - 1, rules))
                {
                    InsertOrAdd(key, value, dictionary);
                }
            }
            InsertOrAdd(input[^1], 1, dictionary);

            return dictionary;
        }

        private static void InsertOrAdd(char key, long count, IDictionary<char, long> dictionary)
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                dictionary[key] = value + count;
            }
            else
            {
                dictionary[key] = count;
            }
        }

        private static string TestInput() => @"NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C";

    }
}
