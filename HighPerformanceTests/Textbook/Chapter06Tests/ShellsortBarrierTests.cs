using System;
using HighPerformance.Textbook.Chapter06;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighPerformanceTests.Textbook.Chapter06Tests
{
    [TestClass]
    public class ShellsortBarrierTests
    {
        private readonly Random _random = new Random();

        [TestMethod]
        public void Test1()
        {
            var a = new int[20];
            for (var i = a.Length - 1; i >= 0; i--)
                a[i] = _random.Next(90) + 10;

            for (var i = a.Length - 1; i >= 0; i--)
                Console.Write(" " + a[i]);
            Console.WriteLine();

            var s = new ShellsortBarrier(3);
            s.SortThem(a);

            for (var i = a.Length - 1; i >= 0; i--)
                Console.Write(" " + a[i]);
            Console.WriteLine();
        }
    }
}