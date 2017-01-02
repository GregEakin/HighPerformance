// Greg Eakin
// January 2, 2017
// Copyright (c) 2017

//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Library General Public
//License as published by the Free Software Foundation; either
//version 2 of the License, or(at your option) any later version.

//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU
//Library General Public License for more details.

//You should have received a copy of the GNU Library General Public
//License along with this library; if not, write to the
//Free Software Foundation, Inc., 59 Temple Place - Suite 330,
//Boston, MA  02111-1307, USA.

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