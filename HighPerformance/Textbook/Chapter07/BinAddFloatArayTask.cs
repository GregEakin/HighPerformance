using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace HighPerformance.Textbook.Chapter07
{
    /// <summary>
    /// Inspired from example 7.2 - nonblocking binary dataflow operation
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