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
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace HighPerformance.Textbook.Chapter07
{
    /// <summary>
    /// C# version Inspaired by the Nonblocking binary dataflow operation, from
    /// Christopher, Thomas W., and George Thiruvathukal. "Example 7-2: nonblocking binary dataflow operation." 
    /// High Performance Java Computing: Multi-threaded and Networked Programming. 
    /// Hemel Hempstead: Prentice Hall, 2000. 205. Print.
    /// </summary>
    public class BinAddFloatArayTask
    {
        private readonly Stopwatch _watch = new Stopwatch();
        private readonly Task<float[]> _left;
        private readonly Task<float[]> _right;

        public BinAddFloatArayTask(Task<float[]> left, Task<float[]> right)
        {
            _watch.Start();

            _left = left;
            _right = right;
            Task = new Task<float[]>(ComputeSum);
            left.ContinueWith(StartTask);
            right.ContinueWith(StartTask);
            Console.WriteLine($"Action created. thread = {Thread.CurrentThread.ManagedThreadId} time = {_watch.ElapsedMilliseconds} ms");
        }

        public Task<float[]> Task { get; }

        public float[] Value => Task.Result;

        private void StartTask(Task<float[]> task)
        {
            if (!_left.IsCompleted)
            {
                Console.WriteLine($"Action waiting on Left. thread = {Thread.CurrentThread.ManagedThreadId} time = {_watch.ElapsedMilliseconds} ms");
                return;
            }

            if (!_right.IsCompleted)
            {
                Console.WriteLine($"Action waiting on Right. thread = {Thread.CurrentThread.ManagedThreadId} time = {_watch.ElapsedMilliseconds} ms");
                return;
            }

            Task.Start();
            Console.WriteLine($"Action started. thread = {Thread.CurrentThread.ManagedThreadId} time = {_watch.ElapsedMilliseconds} ms");
        }

        private float[] ComputeSum()
        {
            var left = _left.Result;
            var right = _right.Result;

            var sum = new float[left.Length];
            for (var i = 0; i < left.Length; i++)
                sum[i] = left[i] + right[i];

            Console.WriteLine($"Action Finised. thread = {Thread.CurrentThread.ManagedThreadId} time = {_watch.ElapsedMilliseconds} ms");
            return sum;
        }
    }
}