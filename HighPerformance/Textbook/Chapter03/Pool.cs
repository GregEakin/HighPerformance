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
using System.Linq;
using System.Threading;

namespace HighPerformance.Textbook.Chapter03
{
    public class Pool
    {
        private readonly List<Buffer> _freeBufferList = new List<Buffer>();
        private readonly int _buffers, _bufferSize;
        private readonly object _sync = new object();

        public Pool(int buffers, int bufferSize)
        {
            _buffers = buffers;
            _bufferSize = bufferSize;
            _freeBufferList.Capacity = buffers;
            for (var i = 0; i < buffers; i++)
                _freeBufferList.Add(new Buffer(bufferSize));
        }

        public Buffer Use()
        {
            lock (_sync)
            {
                while (_freeBufferList.Count == 0)
                    Monitor.Wait(_sync);

                var nextBuffer = _freeBufferList.Last();
                _freeBufferList.Remove(nextBuffer);
                return nextBuffer;
            }
        }

        public void Release(Buffer oldBuffer)
        {
            lock (_sync)
            {
                if (_freeBufferList.Count == 0)
                    Monitor.Pulse(_sync);

                else if (_freeBufferList.Contains(oldBuffer))
                    return;

                _freeBufferList.Add(oldBuffer);
            }
        }
    }
}