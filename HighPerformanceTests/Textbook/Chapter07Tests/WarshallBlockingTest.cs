// Greg Eakin
// January 2, 2017

using HighPerformance.Textbook.Chapter07;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighPerformanceTests.Textbook.Chapter07Tests
{
    [TestClass]
    public class WarshallBlockingTest
    {
        [TestMethod]
        public void Test1()
        {
            var data = new bool[3, 3];
            data[0, 1] = true;
            data[1, 2] = true;
            
            WarshallBlocking.Closure(data);

            Assert.IsTrue(data[0,2]);
        }
    }
}
