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

using System.Collections.Generic;
using System.Threading;

namespace HighPerformance.Textbook.Chapter03
{
    public class BufferQueue
    {
        private readonly object _lock = new object();

        public List<Buffer> Buffers { get; } = new List<Buffer>();

        public void EnqueueBuffer(Buffer buffer)
        {
            lock (_lock)
            {
                if (Buffers.Count == 0)
                    Monitor.Pulse(_lock);
                Buffers.Add(buffer);
            }
        }

        public Buffer DequeueBuffer()
        {
            lock (_lock)
            {
                while (Buffers.Count == 0)
                    Monitor.Pulse(_lock);
                var buffer = Buffers[0];
                Buffers.RemoveAt(0);
                return buffer;
            }
        }
    }
}