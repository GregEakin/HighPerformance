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

using System.IO;
using HighPerformance.Textbook.Chapter03;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighPerformanceTests.Textbook.Chapter03
{
    [TestClass]
    public class FileCopyTests
    {
        [TestMethod]
        public void Test1()
        {
            var pool = new Pool(5, 100);
            var copyBuffers = new BufferQueue();

            var data = new byte[] { 0xA5, 0x5A };
            var source = new MemoryStream(data);
            using (var reader = new FileCopyReader1(source, pool, copyBuffers))
            {
                reader.Run();
            }

            var dest = new MemoryStream();
            using (var writer = new FileCopyWriter1(dest, pool, copyBuffers))
            {
                writer.Run();
            }

            var result = dest.ToArray();
            CollectionAssert.AreEqual(data, result);
        }
    }
}