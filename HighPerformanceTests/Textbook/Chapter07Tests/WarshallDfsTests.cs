﻿// Greg Eakin
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

namespace HighPerformanceTests.Textbook.Chapter07Tests
{
    [TestClass]
    public class WarshallDfsTests
    {
        [TestMethod]
        public void Test1()
        {
            var data = new bool[3][];
            for (var i = 0; i < data.Length; i++)
                data[i] = new bool[3];
            data[0][1] = true;
            data[1][2] = true;

           var result = WarshallDfs.Closure(data);

            Assert.IsTrue(result[0][2]);
        }
    }
}