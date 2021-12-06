using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2021
{
    [TestClass]
    public class Day6 : AoCTestClass
    {
        [TestMethod]
        public void Part1()
        {
            var dayInput = DayInput();
            ISet<Lanternfish> sea = new HashSet<Lanternfish>();
            foreach (var lanternfish in dayInput.Split(',').Select(x => new Lanternfish(int.Parse(x), sea)))
            {
                sea.Add(lanternfish);
            }

            for (int i = 0; i < 80; i++)
            {
                foreach (var lanternfish in sea.ToList())
                {
                    lanternfish.Step();
                }
            }
            TestContext.WriteLine($"Count: {sea.Count}");
        }

        [TestMethod]
        public void Part2()
        {
            var dayInput = DayInput();
            ISet<Lanternfish2> sea = new HashSet<Lanternfish2>();
            foreach (var timer in dayInput.Split(',').Select(int.Parse))
            {
                AddFish(timer, 1, sea);
            }

            for (int i = 0; i < 256; i++)
            {
                foreach (var lanternfish in sea.OrderByDescending(f => f.Timer).ToList())
                {
                    lanternfish.Step();
                }
            }
            TestContext.WriteLine($"Count: {sea.Sum(f => f.Amount)}");
        }

        public static void AddFish(int timer, long amountToAdd, ISet<Lanternfish2> sea)
        {
            if (sea.FirstOrDefault(f => f.Timer == timer) is { } fish)
                fish.Amount += amountToAdd;
            else
                sea.Add(new Lanternfish2(timer, amountToAdd, sea));
        }
    }

    [DebuggerDisplay("{m_timer}")]
    public class Lanternfish
    {
        private int m_timer;
        private readonly ISet<Lanternfish> m_sea;

        public Lanternfish(int timer, ISet<Lanternfish> sea)
        {
            m_timer = timer;
            m_sea = sea;
        }

        public void Step()
        {
            m_timer--;
            if (m_timer >= 0)
                return;

            m_timer = 6;
            m_sea.Add(new Lanternfish(8, m_sea));
        }
    }

    [DebuggerDisplay("{Timer}:{Amount}")]
    public class Lanternfish2
    {
        private readonly ISet<Lanternfish2> m_sea;

        public long Amount { get; set; }

        public int Timer { get; set; }

        public Lanternfish2(int timer, long amount, ISet<Lanternfish2> sea)
        {
            Amount = amount;
            Timer = timer;
            m_sea = sea;
        }

        public void Step()
        {
            Timer--;
            if (Timer >= 0)
                return;

            Day6.AddFish(8, Amount, m_sea);

            if (m_sea.FirstOrDefault(f => f.Timer == 6) is { } fish)
            {
                fish.Amount += Amount;
                m_sea.Remove(this);
            }
            else
                Timer = 6;

        }
    }
}
