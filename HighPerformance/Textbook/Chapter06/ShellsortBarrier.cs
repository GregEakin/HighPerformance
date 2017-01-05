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

using System.Threading;

namespace HighPerformance.Textbook.Chapter06
{
    public class ShellsortBarrier
    {
        private const int MinDivisible = 3;
        private readonly int _threads;

        public ShellsortBarrier(int threads)
        {
            _threads = threads;
        }

        private class Sort
        {
            private readonly int[] _a;
            private readonly Barrier _barrier;
            private readonly int _i;
            private readonly int _threads;
            private int _h;

            public Sort(int[] a, int i, int h, Barrier b, int threads)
            {
                _a = a;
                _i = i;
                _h = h;
                _barrier = b;
                _threads = threads;
            }

            public void Run()
            {
                while (_h > 0)
                {
                    if (_h == 2)
                        _h = 1;
                    for (var m = _i; m < _h; m += _threads)
                        BubbleSort(_a, _i, _h);
                    _h = (int)(_h / 2.2);
                    _barrier.SignalAndWait();
                }
            }
        }

        private static void BubbleSort(int[] a, int m, int h)
        {
            for (var j = m + h; j < a.Length; j += h)
            {
                for (var i = j; i > m && a[i] > a[i - h]; i -= h)
                {
                    var tmp = a[i];
                    a[i] = a[i - h];
                    a[i - h] = tmp;
                }
            }
        }

        public void Shellsort(int[] a)
        {
            if (a.Length < MinDivisible)
            {
                BubbleSort(a, 0, 1);
                return;
            }

            var b = new Barrier(_threads);
            for (var i = _threads - 1; i > 0; i--)
            {
                var sort = new Sort(a, i, a.Length / MinDivisible, b, _threads);
                var thread = new Thread(sort.Run);
                thread.Start();
            }

            new Sort(a, 0, a.Length / MinDivisible, b, _threads).Run();
        }
    }
}