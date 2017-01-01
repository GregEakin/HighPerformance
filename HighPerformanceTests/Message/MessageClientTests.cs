using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighPerformanceTests.Message
{
    [TestClass]
    public class MessageClientTests
    {
        [TestMethod]
        public void AddressTest()
        {
            var address = MessageClient.GetAddress("localhost");
            Assert.AreEqual("127.0.0.1", address.ToString());
        }
    }
}