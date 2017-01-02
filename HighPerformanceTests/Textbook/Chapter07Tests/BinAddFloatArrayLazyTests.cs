using System;
using HighPerformance.Textbook.Chapter07;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighPerformanceTests.Textbook.Chapter07Tests
{
    [TestClass]
    public class BinAddFloatArrayLazyTests
    {
        [TestMethod]
        public void Test1()
        {
            var left = new Lazy<float[]>(() => new[] { 0.0f, 1.0f });
            var right = new Lazy<float[]>(() => new[] { 2.0f, 3.0f });
            var binAdd = new BinAddFloatArrayLazy(left, right);
            Assert.IsFalse(binAdd.Result.IsValueCreated);
            CollectionAssert.AreEqual(new[] { 2.0f, 4.0f }, binAdd.Result.Value);
        }
    }
}