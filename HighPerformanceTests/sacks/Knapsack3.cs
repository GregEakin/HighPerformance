// Greg Eakin
// December 29, 2016

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;

namespace HighPerformanceTests.sacks
{
    public class Knapsack3
    {
        public struct Item
        {
            public int Profit, Weight, Position;
            public float ProfitPerWeight;
        }

        private readonly Item[] _items;

        private readonly int _capacity;

        private BitArray _selected;

        private volatile float _bestProfit;

        // Future done = new Future();

        // SharedTerminationGroup tg = new SharedTerminationGroup(done);

        public BitArray Selected // throws InterruptedException
        {
            get
            {
                // done.Value;
                // rq.MaxThreadsWaiting = 0;
                var s = new BitArray(_items.Length);
                for (var i = 0; i < _items.Length; i++)
                {
                    if (_selected.Get(i))
                        s.Set(_items[i].Position, true);
                }
                return s;
            }
        }

        public int Profit // throws InterruptedException
        {
            get
            {
                // done.getValue();
                // rq.MaxThreadsWaiting = 0;
                return (int)_bestProfit;
            }
        }

        public Knapsack3(int[] weights, int[] profits, int capacity)
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

            // if (levels > _items.Length) levels = _itmes.Length;
            // rq.WaitTime = 10000;
            // rq.MaxThreadCreated = 4;
            // gen (0, capacity, 0, net BitSet());
            // rq.terminate();
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
            // new Thread(new Search(0, capacity, 0, new BitArray(_items.Length) /*, tg*/)).Start();
            DepthFirstSearch(new BitArray(_items.Length), 0, _capacity, 0);
        }
    }

    [TestClass]
    public class TestKnapsack3
    {
        private readonly Random _random = new Random();

        [TestMethod]
        public void Knapsack3Test()
        {
            const int num = 20;
            const int max = 100;
            const int capacity = (int)(num * (max / 2.0) * 0.5);

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

            var ks = new Knapsack3(w, p, capacity);
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