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
    /// Christopher, Thomas W., and George Thiruvathukal. "WarshallC1 and Warshall's Algorithm in Dataflow." 
    /// High Performance Java Computing: Multi-threaded and Networked Programming. 
    /// Hemel Hempstead: Prentice Hall, 2000. 224-27. Print.
    /// </summary>
    public class WarshallC3
    {
        private sealed class Row
        {
            private readonly bool[] _row;
            private readonly int _myRowNumber;
            private readonly bool[][] _rowKStepK;
            private readonly bool[][] _done;
            int j, k;
            private readonly Task<bool[]> _task;

            public Row(bool[] row, int myRowNumber, bool[][] rowKStepK, bool[][] done)
            {
                _row = row;
                _myRowNumber = myRowNumber;
                _rowKStepK = rowKStepK;
                _done = done;

                _task = new Task<bool[]>(ComputeSum);
            }

            public bool[] Value => _task.Result;

            public Task<bool[]> Task1 => _task;

            private void StartTask(Task<bool[]> task)
            {
            }

            private bool[] ComputeSum()
            {
                return null;
            }

            private class Loop
            {
                private readonly bool[][] row_k_step_k;
                private readonly int myRowNumber;
                private readonly bool[] row;
                private readonly bool[][] done;
                private int k;

                public void run()
                {
                    //while(true)
                    //{
                    //    if (k >= row_k_step_k.Length)
                    //    {
                    //        var result = (bool[][])done.getData();
                    //        result[myRowNumber] = row;
                    //        done.signal();
                    //        return;
                    //    }
                    //    if (k == myRowNumber)
                    //    {
                    //        row_k_step_k[k].setValue(row.Clone());
                    //        k++;
                    //        continue;
                    //    }
                    //    if (!row[k])
                    //    {
                    //        k++;
                    //        continue;
                    //    }
                    //    row_k_step_k[k].runDelayed(updateRow);
                    //    return;
                    //}
                }
            }

            private readonly Loop loop = new Loop();

            private class UpdateRow
            {
                private readonly bool[][] row_k_step_k;
                private readonly bool[] row;
                private int k;

                public void run()
                {
                    //bool[] row_k = (bool[])row_k_step_k[k].getValue();
                    //for (j = 0; j < row.Length; j++)
                    //{
                    //    row[j] |= row_k[j];
                    //}
                    //k++;
                    //rq.run(loop);
                }
            }

            private readonly UpdateRow updateRow = new UpdateRow();

            public void Run()
            {
                //k = 0;
                //rq.run(loop);
            }
        }

        public static bool[][] Closure(bool[][] a)
        {
            var size = a.Length;
            for (var i = 0; i < size; i++)
                if (size != a[i].Length)
                    throw new ArgumentException(nameof(a));

            // RunQueue rq = new RunQueue(numThreads);
            // FutureFactory ff = new FutureFactory(rq);

            var kthRows = new bool[size][];
            //for (var i = 0; i < size; ++i)
            //    kthRows[i] = ff.make();

            var done = new bool[size][];
            var tasks = new Task[size];
            for (var i = 0; i < size; i++)
            {
                var row = new Row((bool[]) a[i].Clone(), i, kthRows, done);
                tasks[i] = row.Task1;
            }

            Task.WaitAll(tasks);
            return done;
        }
    }
}