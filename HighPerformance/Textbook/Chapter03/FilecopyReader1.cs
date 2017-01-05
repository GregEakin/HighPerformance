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
    public class FileCopyReader1 : IDisposable
    {
        private readonly Pool _pool;
        private readonly BufferQueue _copyBuffers;
        private readonly BinaryReader _reader;

        public FileCopyReader1(Stream input, Pool pool, BufferQueue copyBuffers)
        {
            _pool = pool;
            _copyBuffers = copyBuffers;
            _reader = new BinaryReader(input);
        }

        public void Run()
        {
            var index = 0;
            int bytesRead;
            do
            {
                Buffer buffer;
                try
                {
                    buffer = _pool.Use();
                    bytesRead = _reader.Read(buffer.BufferData, index, buffer.Length);
                    index += bytesRead;
                    buffer.Length = bytesRead;
                }
                catch (Exception)
                {
                    buffer = new Buffer(0);
                    bytesRead = 0;
                }

                _copyBuffers.EnqueueBuffer(buffer);
            } while (bytesRead > 0);
        }

        public void Dispose()
        {
            _reader?.Dispose();
        }
    }
}