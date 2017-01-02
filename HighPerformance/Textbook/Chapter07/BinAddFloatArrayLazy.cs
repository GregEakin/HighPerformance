// Greg Eakin
// December 30, 2016

using System;

namespace HighPerformance.Textbook.Chapter07
{
    /// <summary>
    /// Inspired from example 7.1 - blocking binary dataflow operation
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

        public float[] Run()
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