using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2021
{
    [TestClass]
    public class Day4 : AoCTestClass
    {
        [TestMethod]
        public void Part1()
        {
            var dayInput = DayInput();
            var stringReader = new StringReader(dayInput);
            string? line = stringReader.ReadLine();
            var numbers = line.Split(',').Select(int.Parse).ToArray();
            var row = 0;
            var board = -1;
            var boards = new List<int[][]>();
            while ((line = stringReader.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                {
                    row = 0;
                    board++;
                    boards.Insert(board, new int[5][]);
                    continue;
                }
                boards[board][row] = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                row++;
            }

            foreach (var (number, i) in numbers.Select((n, i) => (n, i)))
            {
                Mark(number, boards);
                if (i >= 5)
                {
                    if (Winner(boards, out var winningBoard))
                    {
                        var sum = winningBoard.Sum(row => row.Where(n => n > 0).Sum());
                        TestContext.WriteLine($"sum: {sum}");
                        TestContext.WriteLine($"number: {number}");
                        TestContext.WriteLine($"Result: {sum * number}");
                        break;
                    }
                }
            }
        }

        [TestMethod]
        public void Part2()
        {
            var dayInput = DayInput();
            var stringReader = new StringReader(dayInput);
            string? line = stringReader.ReadLine();
            var numbers = line.Split(',').Select(int.Parse).ToArray();
            var row = 0;
            var board = -1;
            var boards = new List<int[][]>();
            while ((line = stringReader.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                {
                    row = 0;
                    board++;
                    boards.Insert(board, new int[5][]);
                    continue;
                }
                boards[board][row] = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                row++;
            }

            foreach (var (number, i) in numbers.Select((n, i) => (n, i)))
            {
                Mark(number, boards);
                if (i >= 5)
                {
                    while (Winner(boards, out var winningBoard))
                    {
                        boards.Remove(winningBoard);
                        if (boards.Count == 0)
                        {
                            var sum = winningBoard.Sum(row => row.Where(n => n > 0).Sum());
                            TestContext.WriteLine($"sum: {sum}");
                            TestContext.WriteLine($"number: {number}");
                            TestContext.WriteLine($"Result: {sum * number}");
                            break;
                        }

                    }
                }
            }
        }

        private bool Winner(List<int[][]> boards, out int[][]? board)
        {
            board = null;
            for (var i = 0; i < boards.Count; i++)
            {
                if (!CheckBoard(boards[i]))
                    continue;

                board = boards[i];
                return true;
            }

            return false;
        }

        private bool CheckBoard(int[][] board)
        {
            for (var i = 0; i < board.Length; i++)
            {
                var horizontalbingo = true;
                var verticalalbingo = true;
                for (var j = 0; j < board[i].Length; j++)
                {
                    if (board[i][j] > 0)
                        horizontalbingo = false;
                    if (board[j][i] > 0)
                        verticalalbingo = false;
                }

                if (horizontalbingo || verticalalbingo)
                    return true;
            }

            return false;
        }

        private void Mark(int number, List<int[][]> boards)
        {
            for (var i = 0; i < boards.Count; i++)
            {
                for (var j = 0; j < boards[i].Length; j++)
                {
                    for (var k = 0; k < boards[i][j].Length; k++)
                    {
                        if (boards[i][j][k] == number)
                            boards[i][j][k] = -number;
                    }
                }
            }
        }

        private const string TestInput = @"7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1

22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10  3 18  5
 1 12 20 15 19

 3 15  0  2 22
 9 18 13 17  5
19  8  7 25 23
20 11 10 24  4
14 21 16 12  6

14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7";
    }
}
