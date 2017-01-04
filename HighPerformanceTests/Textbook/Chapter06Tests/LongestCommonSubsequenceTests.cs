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
    public class LongestCommonSubsequenceTests
    {
        [TestMethod]
        public void Test1()
        {
            var s0 = "String1i";
            var s1 = "ingrings";
            var nt = 4;
            var lcs = new LongestCommonSubsequence(s0, s1, nt);

            Assert.AreEqual(4, lcs.Length);
            var a = lcs.Array;
            for (var i = 0; i < a.GetLength(0); i++)
            {
                for (var j = 0; j < a.GetLength(1); j++)
                    Console.Write($"{a[i, j]} ");

                Console.WriteLine();
            }
        }
    }
}
