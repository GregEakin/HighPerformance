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

using HighPerformance.Textbook.Chapter07;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace HighPerformanceTests.Textbook.Chapter07Tests
{
    [TestClass]
    public class BinAddFloatArayTaskTests
    {
        [TestMethod]
        public void CompletedTasksTest()
        {
            var left = Task.FromResult(new[] { 0.0f, 1.0f });
            var right = Task.FromResult(new[] { 2.0f, 3.0f });
            var binAdd = new BinAddFloatArayTask(left, right);

            var floats = binAdd.Value;
            CollectionAssert.AreEqual(new[] { 2.0f, 4.0f }, floats);
        }

        [TestMethod]
        public void RunningTasksTest()
        {
            var left = new Task<float[]>(() => new[] { 0.0f, 1.0f });
            var right = new Task<float[]>(() => new[] { 2.0f, 3.0f });
            var binAdd = new BinAddFloatArayTask(left, right);

            Thread.Sleep(100);
            left.Start();

            Thread.Sleep(100);
            right.Start();

            var floats = binAdd.Value;
            CollectionAssert.AreEqual(new[] { 2.0f, 4.0f }, floats);
        }

        [TestMethod]
        public void ThreeWayTest()
        {
            var left = new Task<float[]>(() => new[] { 0.0f, 1.0f });
            var center = new Task<float[]>(() => new[] { -4.0f, 99.0f });
            var right = new Task<float[]>(() => new[] { 2.0f, 3.0f });
            var binAdd1 = new BinAddFloatArayTask(left, right);
            var binAdd2 = new BinAddFloatArayTask(center, binAdd1.Task);

            center.Start();
            left.Start();
            right.Start();

            var floats = binAdd2.Value;
            CollectionAssert.AreEqual(new[] { -2.0f, 103.0f }, floats);
        }
    }
}