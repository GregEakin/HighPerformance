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

using HighPerformance.Textbook.Chapter07;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighPerformanceTests.Textbook.Chapter07Tests
{
    [TestClass]
    public class WarshallC1Tests
    {
        [TestMethod]
        public void Test1()
        {
            const int size = 3;
            var data = new bool[size][];
            for (var i = 0; i < size; i++)
                data[i] = new bool[size];
            data[0][1] = true;
            data[1][2] = true;

            var result = WarshallC1.Closure(data);

            Assert.IsTrue(result[0][2]);
        }

        [TestMethod]
        public void DiagonalTest()
        {
            const int size = 33;
            var data = new bool[size][];
            for (var i = 0; i < size; i++)
                data[i] = new bool[size];

            for (var i = 0; i < size; i++)
                data[i][(i + 1) % size] = true;

            var result = WarshallC1.Closure(data);

            for (var i = 0; i < size; i++)
                for (var j = 0; j < size; j++)
                    Assert.IsTrue(result[i][j]);
        }
    }
}