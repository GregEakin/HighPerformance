// Greg Eakin
// January 2, 2017

using System;

namespace HighPerformance.Textbook.Chapter07
{
    public class WarshallBlocking
    {
        /// <summary>
        /// Inspaired by Example 7-17 Warshall's algorithm
        /// </summary>
        /// <param name="data">The 2-d array of paths through the graph to be reduced.</param>
        public static void WarshallAlgorithm(bool[,] data)
        {
            var size = data.GetLength(0);
            if (size != data.GetLength(1))
                throw new ArgumentException(nameof(data));

            for (var k = 0; k < size; k++)
                for (var i = 0; i < size; i++)
                    if (data[i, k])
                        for (var j = 0; j < size; j++)
                            data[i, j] = data[i, j] || data[k, j];
        }

    }
}