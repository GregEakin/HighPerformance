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

namespace HighPerformance.Textbook.Chapter07
{
    /// <summary>
    /// C# version inspaired by the Blocking binary dataflow operation, from
    /// Christopher, Thomas W., and George Thiruvathukal. "Example 7-1: blocking binary dataflow operation." 
    /// High Performance Java Computing: Multi-threaded and Networked Programming. 
    /// Hemel Hempstead: Prentice Hall, 2000. 205. Print.
    /// </summary>
    public class BinAddFloatArrayLazy
    {
        private readonly Lazy<float[]> _left;
        private readonly Lazy<float[]> _right;

        public BinAddFloatArrayLazy(Lazy<float[]> left, Lazy<float[]> right)
        {
            _left = left;
            _right = right;
            Result = new Lazy<float[]>(Run);
        }

        public Lazy<float[]> Result { get; }

        private float[] Run()
        {
            var left = _left.Value;
            var right = _right.Value;
            var sum = new float[left.Length];
            for (var i = 0; i < left.Length; i++)
                sum[i] = left[i] + right[i];
            return sum;
        }
    }
}