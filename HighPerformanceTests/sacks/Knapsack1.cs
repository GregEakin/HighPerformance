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
            public int profit, weight, pos;
            public float profitPerWeight;
        }

        private readonly Item[] _items;

        private BitArray _selected;

        private int _capacity;

        private float _bestProfit;

        // Future done = new Future();

        // SharedTerminationGroup tg = new SharedTerminationGroup(done);

        public BitArray Selected // throws InterruptedException
        {
            get
            {
                // done.getValue();
                var s = new BitArray(_items.Length);
                for (var i = 0; i < _items.Length; i++)
                {
                    if (_selected.Get(i))
                        s.Set(_items[i].pos, true);
                }
                return s;
            }
        }

        public int Profit // throws InterruptedException
        {
            get
            {
                // done.getValue();
                return (int)_bestProfit;
            }
        }

        public Knapsack1(int[] weights, int[] profits, int capacity)
        {
            if (weights.Length != profits.Length)
                throw new Exception("0/1 Knapsack: differing numbers of weights and profits");
            if (capacity <= 0)
                throw new Exception("0/1 Knapsack: capacity<=0");
            _items = new Item[weights.Length];
            for (var i = 0; i < _items.Length; i++)
            {
                _items[i] = new Item()
                {
                    profit = profits[i],
                    weight = weights[i],
                    pos = i,
                    profitPerWeight = ((float)profits[i]) / weights[i]
                };
            }

            for (var j = 1; j < _items.Length; j++)
            {
                for (var i = j; i > 0 && _items[i].profitPerWeight > _items[i - 1].profitPerWeight; i--)
                {
                    var tmp = _items[i];
                    _items[i] = _items[i - 1];
                    _items[i - 1] = tmp;
                }
            }

            // new Thread(new Search(0, capacity, 0, new BitArray(_items.Length) /*, tg*/)).Start();
            var search = new Search(0, capacity, 0, new BitArray(_items.Length));
            DepthFirstSearch(search, search.from, search.startWeight, search.startProfit);
        }

        public class Search // implements Runnable
        {
            public readonly BitArray selected;

            public readonly int from;

            public readonly int startWeight;

            public readonly int startProfit;

            // SharedTerminationGroup tg;

            public Search(int from, int remainingWeight, int profit, BitArray selected /*, SharedTerminationGroup tg*/)
            {
                this.from = from;
                startWeight = remainingWeight;
                startProfit = profit;
                this.selected = selected;
                // this.tg = tg;
            }
        }

        void DepthFirstSearch(Search search, int i, int rw, int p)
        {
            if (i >= _items.Length)
            {
                if (p <= _bestProfit) return;
                _bestProfit = p;
                _selected = new BitArray(search.selected);
                Console.WriteLine($"new best: {p}");
                return;
            }

            if (p + rw * _items[i].profitPerWeight < _bestProfit)
                return;

            if (rw - _items[i].weight >= 0)
            {
                search.selected.Set(i, true);
                DepthFirstSearch(search, i + 1, rw - _items[i].weight, p + _items[i].profit);
            }

            search.selected.Set(i, false);
            DepthFirstSearch(search, i + 1, rw, p);
        }

        public void Run()
        {
            // dfs(from, startWeight, startProfit);
            // tg.terminate();
        }
    }

    [TestClass]
    public class TestKnapsack1
    {
        private readonly Random _random = new Random();

        [TestMethod]
        public void Test1()
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

            var ks = new Knapsack1(w, p, capacity);
            var s = ks.Selected;
            Console.Write("s:");
            for (var i = p.Length - 1; i >= 0; i--)
                Console.Write($" {(s.Get(i) ? '1' : '0')} ");
            Console.WriteLine();

            Console.WriteLine($"Profit: {ks.Profit}");
        }
    }
}