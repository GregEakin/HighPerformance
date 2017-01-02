// Greg Eakin
// January 2, 2017
// Copyright (c) 2017

//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Library General Public
//License as published by the Free Software Foundation; either
//version 2 of the License, or(at your option) any later version.

//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU
//Library General Public License for more details.

//You should have received a copy of the GNU Library General Public
//License along with this library; if not, write to the
//Free Software Foundation, Inc., 59 Temple Place - Suite 330,
//Boston, MA  02111-1307, USA.

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