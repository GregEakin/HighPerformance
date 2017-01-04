// High Performance C# Multithreading Programming
// Copyright(C) 2017  Greg Eakin

// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License along
// with this program; if not, write to the Free Software Foundation, Inc.,
// 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.

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