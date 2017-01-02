// Greg Eakin
// January 2, 2017

using HighPerformance.Textbook.Chapter07;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace HighPerformanceTests.Textbook.Chapter07Tests
{
    [TestClass]
    public class BinAddFloatArayTaskTests
    {
        [TestMethod]
        public void CompletedTasksTest()
        {
            var left = Task.FromResult(new[] { 0.0f, 1.0f });
            var right = Task.FromResult(new[] { 2.0f, 3.0f });
            var binAdd = new BinAddFloatArayTask(left, right);

            var floats = binAdd.Value;
            CollectionAssert.AreEqual(new[] { 2.0f, 4.0f }, floats);
        }

        [TestMethod]
        public void RunningTasksTest()
        {
            var left = new Task<float[]>(() => new[] { 0.0f, 1.0f });
            var right = new Task<float[]>(() => new[] { 2.0f, 3.0f });
            var binAdd = new BinAddFloatArayTask(left, right);

            Thread.Sleep(100);
            left.Start();

            Thread.Sleep(100);
            right.Start();

            var floats = binAdd.Value;
            CollectionAssert.AreEqual(new[] { 2.0f, 4.0f }, floats);
        }

        [TestMethod]
        public void ThreeWayTest()
        {
            var left = new Task<float[]>(() => new[] { 0.0f, 1.0f });
            var center = new Task<float[]>(() => new[] { -4.0f, 99.0f });
            var right = new Task<float[]>(() => new[] { 2.0f, 3.0f });
            var binAdd1 = new BinAddFloatArayTask(left, right);
            var binAdd2 = new BinAddFloatArayTask(center, binAdd1.Task);

            center.Start();
            left.Start();
            right.Start();

            var floats = binAdd2.Value;
            CollectionAssert.AreEqual(new[] { -2.0f, 103.0f }, floats);
        }
    }
}