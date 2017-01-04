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

        public void SortThem(int[] a)
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