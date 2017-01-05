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
using System.Threading.Tasks;

namespace HighPerformance.Textbook.Chapter07
{
    /// <summary>
    /// Inspaired by the Warshall's algorithm, from
    /// Christopher, Thomas W., and George Thiruvathukal. "WarshallDF1 and Warshall's Algorithm in Dataflow." 
    /// High Performance Java Computing: Multi-threaded and Networked Programming. 
    /// Hemel Hempstead: Prentice Hall, 2000. 218-23. Print.
    /// </summary>
    public class WarshallDfs
    {
        private sealed class RowUpdate
        {
            private readonly int _k;
            private readonly RowUpdate _myRow;
            private readonly RowUpdate _kthRow;
            private readonly Task<bool[]> _task;

            public RowUpdate(bool[] v)
            {
                _task = Task.FromResult(v);
            }

            public RowUpdate(RowUpdate myRow, RowUpdate kthRow, int k)
            {
                _myRow = myRow;
                _kthRow = kthRow;
                _k = k;
                _task = new Task<bool[]>(ComputeSum);
                myRow._task.ContinueWith(StartTask);
                kthRow._task.ContinueWith(StartTask);
            }

            public bool[] Value => _task.Result;

            private void StartTask(Task<bool[]> task)
            {
                if (!_myRow._task.IsCompleted)
                    return;

                var row = _myRow.Value;
                if (!row[_k])
                {
                    _task.RunSynchronously();
                    return;
                }

                if (!_kthRow._task.IsCompleted)
                    return;

                _task.Start();
            }

            private bool[] ComputeSum()
            {
                var row = _myRow.Value;
                if (!row[_k])
                    return row;

                var rowK = _kthRow.Value;
                var result = new bool[rowK.Length];
                for (var j = 0; j < rowK.Length; j++)
                    result[j] = row[j] || rowK[j];

                return result;
            }
        }

        public static bool[][] Closure(bool[][] data)
        {
            var size = data.Length;
            for (var i = 0; i < size; i++)
                if (size != data[i].Length)
                    throw new ArgumentException(nameof(data));

            var sourceRows = new RowUpdate[size];
            for (var i = 0; i < size; i++)
                sourceRows[i] = new RowUpdate(data[i]);

            for (var k = 0; k < size; k++)
            {
                var destRows = new RowUpdate[size];
                for (var i = 0; i < size; i++)
                {
                    destRows[i] = i == k
                        ? sourceRows[i]
                        : new RowUpdate(sourceRows[i], sourceRows[k], k);
                }

                sourceRows = destRows;
            }

            var result = new bool[size][];
            for (var i = 0; i < size; i++)
                result[i] = sourceRows[i].Value;

            // wait for all to finish.
            return result;
        }
    }
}
