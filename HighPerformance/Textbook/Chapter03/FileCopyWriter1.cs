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
using System.IO;

namespace HighPerformance.Textbook.Chapter03
{
    public class FileCopyWriter1 : IDisposable
    {
        private readonly Pool _pool;
        private readonly BufferQueue _copyBuffers;
        private readonly BinaryWriter _writer;

        public FileCopyWriter1(Stream output, Pool pool, BufferQueue copyBuffers)
        {
            _pool = pool;
            _copyBuffers = copyBuffers;
            _writer = new BinaryWriter(output);
        }

        public void Run()
        {
            while (true)
            {
                var buffer = _copyBuffers.DequeueBuffer();
                try
                {
                    if (buffer.Length <= 0)
                        break;
                    var data = buffer.BufferData;
                    var size = buffer.Length;
                    _writer.Write(data, 0, size);
                }
                catch (Exception)
                {
                    break;
                }
                finally
                {
                    _pool.Release(buffer);
                }
            }
        }

        public void Dispose()
        {
            _writer?.Dispose();
        }
    }
}