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
using System.Threading;

namespace HighPerformance.Textbook.Chapter07
{
    /// <summary>
    /// Inspaired by the Warshall's algorithm, from
    /// Christopher, Thomas W., and George Thiruvathukal. "WarshallC1 and Warshall's Algorithm in Dataflow." 
    /// High Performance Java Computing: Multi-threaded and Networked Programming. 
    /// Hemel Hempstead: Prentice Hall, 2000. 224-27. Print.
    /// </summary>
    public class WarshallC1
    {
        private sealed class Row
        {
            private readonly bool[] _row;
            private readonly int _myRowNumber;
            private readonly bool[][] _rowKStepK;
            private readonly bool[][] _done;

            public Row(bool[] row, int myRowNumber, bool[][] rowKStepK, bool[][] done)
            {
                _row = row;
                _myRowNumber = myRowNumber;
                _rowKStepK = rowKStepK;
                _done = done;
                ManualEvent = new ManualResetEvent(false);
            }

            public ManualResetEvent ManualEvent { get; }

            public void Run()
            {
                for (var k = 0; k < _rowKStepK.Length; k++)
                {
                    if (k == _myRowNumber)
                        lock (_rowKStepK)
                        {
                            _rowKStepK[k] = (bool[])_row.Clone();
                            Monitor.PulseAll(_rowKStepK);
                        }
                    else if (_row[k])
                    {
                        bool[] rowK;
                        lock (_rowKStepK)
                        {
                            while (_rowKStepK[k] == null)
                                Monitor.Wait(_rowKStepK);

                            rowK = _rowKStepK[k];
                        }
                        for (var j = 0; j < _row.Length; j++)
                            _row[j] |= rowK[j];
                    }
                }

                _done[_myRowNumber] = _row;
                ManualEvent.Set();
            }
        }

        public static bool[][] Closure(bool[][] data)
        {
            var size = data.Length;
            for (var i = 0; i < size; i++)
                if (size != data[i].Length)
                    throw new ArgumentException(nameof(data));

            var kthRows = new bool[size][];

            var threads = new WaitHandle[size];
            var done = new bool[size][];
            for (var i = 0; i < size; i++)
            {
                var row = new Row((bool[])data[i].Clone(), i, kthRows, done);
                threads[i] = row.ManualEvent;
                var thread = new Thread(row.Run);
                thread.Start();
            }

            foreach (var e in threads)
                e.WaitOne();

            return done;
        }
    }
}