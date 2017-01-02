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
using HighPerformance.Textbook.Chapter07;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighPerformanceTests.Textbook.Chapter07Tests
{
    [TestClass]
    public class BinAddFloatArrayLazyTests
    {
        [TestMethod]
        public void Test1()
        {
            var left = new Lazy<float[]>(() => new[] { 0.0f, 1.0f });
            var right = new Lazy<float[]>(() => new[] { 2.0f, 3.0f });
            var binAdd = new BinAddFloatArrayLazy(left, right);
            Assert.IsFalse(binAdd.Result.IsValueCreated);
            CollectionAssert.AreEqual(new[] { 2.0f, 4.0f }, binAdd.Result.Value);
        }
    }
}