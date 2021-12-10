using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2021
{
    [TestClass]
    public class Day10 : AoCTestClass
    {
        [TestMethod]
        public void Part1()
        {
            var dayInput = DayInput();
            var stringReader = new StringReader(dayInput);
            string? line;
            var sum = 0;
            var points = new Dictionary<char, int> { { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 } };
            var pair = new Dictionary<char, char> { { ')', '(' }, { ']', '[' }, { '}', '{' }, { '>', '<' } };
            var stack = new Stack<char>();
            while ((line = stringReader.ReadLine()) != null)
            {
                foreach (var character in line)
                {
                    if (pair.ContainsValue(character))
                    {
                        stack.Push(character);
                        continue;
                    }

                    if (pair.ContainsKey(character))
                    {
                        if (stack.Count > 0 && stack.Peek() == pair[character])
                        {
                            stack.Pop();
                        }
                        else
                        {
                            sum += points[character];
                            break;
                        }
                    }
                }
            }
            TestContext.WriteLine($"Points: {sum}");
        }

        [TestMethod]
        public void Part2()
        {
            var dayInput = DayInput();
            var stringReader = new StringReader(dayInput);
            string? line;
            var result = new List<long>();
            var points = new Dictionary<char, int> { { '(', 1 }, { '[', 2 }, { '{', 3 }, { '<', 4 } };
            var pair = new Dictionary<char, char> { { ')', '(' }, { ']', '[' }, { '}', '{' }, { '>', '<' } };
            var stack = new Stack<char>();
            while ((line = stringReader.ReadLine()) != null)
            {
                foreach (var character in line)
                {
                    if (pair.ContainsValue(character))
                    {
                        stack.Push(character);
                        continue;
                    }

                    if (pair.ContainsKey(character))
                    {
                        if (stack.Count > 0 && stack.Peek() == pair[character])
                        {
                            stack.Pop();
                        }
                        else
                        {
                            stack.Clear();
                            break;
                        }
                    }
                }

                var score = 0L;
                while (stack.Count > 0)
                {
                    var point = points[stack.Pop()];
                    score = 5 * score + point;
                }

                if (score > 0)
                    result.Add(score);
            }

            var finalScore = result.OrderBy(r => r).Skip(result.Count / 2).First();
            TestContext.WriteLine($"Points: {finalScore}");
        }

        private static string TestInput() => @"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]";

    }
}
