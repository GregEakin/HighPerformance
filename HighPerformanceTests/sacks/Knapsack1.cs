// Greg Eakin
// December 29, 2016

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;

namespace HighPerformanceTests.sacks
{
    public class Knapsack1
    {
        public struct Item
        {
            public int Profit, Weight, Position;
            public float ProfitPerWeight;
        }

        private readonly Item[] _items;

        private readonly int _capacity;

        private BitArray _selected;

        private float _bestProfit;

        public BitArray Selected
        {
            get
            {
                var s = new BitArray(_items.Length);
                for (var i = 0; i < _items.Length; i++)
                    s.Set(_items[i].Position, _selected.Get(i));
                return s;
            }
        }

        public int Profit => (int)_bestProfit;

        public Knapsack1(int[] weights, int[] profits, int capacity)
        {
            if (weights.Length != profits.Length)
                throw new ArgumentException("0/1 Knapsack: differing numbers of weights and profits");

            if (capacity <= 0)
                throw new ArgumentException("0/1 Knapsack: capacity <= 0", nameof(capacity));

            _capacity = capacity;
            _items = new Item[weights.Length];
            for (var i = 0; i < _items.Length; i++)
            {
                _items[i] = new Item()
                {
                    Profit = profits[i],
                    Weight = weights[i],
                    Position = i,
                    ProfitPerWeight = ((float)profits[i]) / weights[i]
                };
            }

            Array.Sort(_items, (item, item1) => item1.ProfitPerWeight.CompareTo(item.ProfitPerWeight));
        }

        private void DepthFirstSearch(BitArray search, int i, int rw, int p)
        {
            if (i >= _items.Length)
            {
                if (p <= _bestProfit) return;

                _bestProfit = p;
                _selected = new BitArray(search);
                Console.WriteLine($"new best: {p}");
                return;
            }

            if (p + rw * _items[i].ProfitPerWeight < _bestProfit)
                return;

            if (rw - _items[i].Weight >= 0)
            {
                search.Set(i, true);
                DepthFirstSearch(search, i + 1, rw - _items[i].Weight, p + _items[i].Profit);
            }

            search.Set(i, false);
            DepthFirstSearch(search, i + 1, rw, p);
        }

        public void Solve()
        {
            _bestProfit = 0.0f;
            _selected = null;
            DepthFirstSearch(new BitArray(_items.Length), 0, _capacity, 0);
        }
    }

    [TestClass]
    public class TestKnapsack1
    {
        private readonly Random _random = new Random();

        [TestMethod]
        public void Knapsack1Test()
        {
            const int num = 20;
            const int max = 100;
            const int capacity = (int)(num * (max / 2.0) * 0.7);

            var p = new int[num];
            var w = new int[num];
            for (var i = p.Length - 1; i >= 0; i--)
            {
                p[i] = 1 + _random.Next(max - 1);
                w[i] = 1 + _random.Next(max - 1);
            }

            Console.Write("p:");
            for (var i = p.Length - 1; i >= 0; i--)
                Console.Write($" {p[i]}");
            Console.WriteLine();

            Console.Write("w:");
            for (var i = p.Length - 1; i >= 0; i--)
                Console.Write($" {w[i]}");
            Console.WriteLine();

            Console.WriteLine($"Capacity {capacity}");
            Console.WriteLine();

            var ks = new Knapsack1(w, p, capacity);
            ks.Solve();

            var selected = ks.Selected;
            Console.Write("s:");
            for (var i = p.Length - 1; i >= 0; i--)
                Console.Write($" {(selected.Get(i) ? '1' : '0')} ");
            Console.WriteLine();

            Console.WriteLine($"Profit: {ks.Profit}");
        }
    }
}