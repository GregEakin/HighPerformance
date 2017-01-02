using System.Threading;
using HighPerformance.Textbook.Chapter07;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighPerformanceTests.Textbook.Chapter07Tests
{
    [TestClass]
    public class WarshallDfsTests
    {
        [TestMethod]
        public void Test1()
        {
            var data = new bool[3][];
            for (var i = 0; i < data.Length; i++)
                data[i] = new bool[3];
            data[0][1] = true;
            data[1][2] = true;

           var result = WarshallDfs.Closure(data);

            Assert.IsTrue(result[0][2]);
        }
    }
}