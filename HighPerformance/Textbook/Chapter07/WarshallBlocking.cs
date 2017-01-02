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
    /// Inspaired by the Warshall's algorithm, from
    /// Christopher, Thomas W., and George Thiruvathukal. "Example 7-17: Warshall's Algorithm." 
    /// High Performance Java Computing: Multi-threaded and Networked Programming. 
    /// Hemel Hempstead: Prentice Hall, 2000. 218-19. Print.
    /// </summary>
    public class WarshallBlocking
    {
        public static void Closure(bool[,] data)
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