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

namespace HighPerformance.Textbook.Chapter06
{
    public class LongestCommonSubsequence
    {
        private readonly char[] _c0;
        private readonly char[] _c1;
        private readonly int[,] _a;
        private readonly Lazy<bool> _done;

        public LongestCommonSubsequence(char[] c0, char[] c1, int numberThreads)
        {
            _c0 = c0;
            _c1 = c1;
            _a = new int[_c0.Length + 1, _c1.Length + 1];

            var bands = new Band[numberThreads];
            _done = new Lazy<bool>(() =>
            {
                foreach (var e in bands)
                    e.ManualEvent.WaitOne();
                return true;
            });

            var left = new SemaphoreSlim(c0.Length);
            for (var i = 0; i < numberThreads; i++)
            {
                var right = new SemaphoreSlim(0);
                var low = StartOfBand(i, numberThreads, _c1.Length);
                var high = StartOfBand(i + 1, numberThreads, _c1.Length) - 1;
                bands[i] = new Band(low, high, left, right, _c0, _c1, _a);
                bands[i].Thread.Start();
                left = right;
            }
        }

        public LongestCommonSubsequence(string s0, string s1, int nt)
            : this(s0.ToCharArray(), s1.ToCharArray(), nt)
        { }

        private class Band
        {
            private readonly int _low;
            private readonly int _high;
            private readonly SemaphoreSlim _left;
            private readonly SemaphoreSlim _right;
            private readonly char[] _c0;
            private readonly char[] _c1;
            private readonly int[,] _a;

            public Band(int low, int high, SemaphoreSlim left, SemaphoreSlim right, char[] c0, char[] c1, int[,] a)
            {
                _low = low;
                _high = high;
                _left = left;
                _right = right;
                _c0 = c0;
                _c1 = c1;
                _a = a;

                Thread = new Thread(Run);
                ManualEvent = new ManualResetEvent(false);
            }

            public ManualResetEvent ManualEvent { get; }

            public Thread Thread { get; }

            private void Run()
            {
                for (var i = 1; i < _a.GetLength(0); i++)
                {
                    _left.Wait();
                    for (var j = _low; j <= _high; j++)
                    {
                        if (_c0[i - 1] == _c1[j - 1])
                            _a[i, j] = _a[i - 1, j - 1] + 1;
                        else
                            _a[i, j] = Math.Max(_a[i - 1, j], _a[i, j - 1]);
                    }
                    _right.Release();
                }

                ManualEvent.Set();
            }
        }

        private static int StartOfBand(int i, int nb, int n)
        {
            return 1 + i * (n / nb) + Math.Min(i, n % nb);
        }

        public int Length
        {
            get
            {
                var x = _done.Value;
                return _a[_c0.Length, _c1.Length];
            }
        }

        public int[,] Array
        {
            get
            {
                var x = _done.Value;
                return _a;
            }
        }
    }
}